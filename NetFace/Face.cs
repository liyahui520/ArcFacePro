using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;//Marshal类别，必须要引用这个
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.ML;
using Emgu.CV.UI;
using ZedGraph; 
using System.Diagnostics; 
using NetFace.Common;

namespace NetFace
{
    public partial class Face : Form
    {
        public Face()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;//干掉检测 不再检测跨线程
            _SyncContext = SynchronizationContext.Current;
        }

        #region 自定义私有

        /// <summary>
        /// EmguCV中获取视频信息的类
        /// </summary>
        Capture capture;

        ThreadInvoker thread;
        int VideoFps1 = 0;

        SynchronizationContext _SyncContext;
        #endregion

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Add("初始化系统中...");
            thread = new ThreadInvoker(AsynUpdateTxtMethod);
            thread.Stop = false;
            thread.Start(new System.Threading.ParameterizedThreadStart(Excete));
        }

        public void AsynUpdateTxtMethod(InvokerClass InvokerClass)
        {
            if (this.listBox1.InvokeRequired)
            {
                this.BeginInvoke(new ThreadInvoker.CallbackFunc(updatemethod), InvokerClass);
            }
            else
            {
                updatemethod(InvokerClass);
            }
        }

        public void Excete(object InvokerClass)
        {
            InvokerClass _InvokerClass = InvokerClass as InvokerClass;
            while (!thread.Stop)
            {
                if (null == _InvokerClass)
                    _InvokerClass = new InvokerClass();
                
                string RTSPStreamText = textBox1.Text.Trim();
                capture = new Capture(RTSPStreamText);
                //capture.SetCaptureProperty(CapProp.Fps, 1);
                // VideoFps1=(int)capture.GetCaptureProperty(CapProp.Fps);
                // capture.GetCaptureProperty(1); 
                //VideoFps1 = CvInvoke.pro.cvGetCaptureProperty().cvGetCaptureProperty(capture, Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FPS);
                //capture.ImageGrabbed += Capture_ImageGrabbed;
              
                _InvokerClass.String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " :   执行中。。。";
                aa();

                thread.AsynCallback(_InvokerClass);
                Thread.Sleep(thread.Sleep);
            }
        }

        public void updatemethod(InvokerClass InvokerClass)
        {
            if (capture == null || capture.Ptr == IntPtr.Zero)
                this.listBox1.Items.Add(InvokerClass.String + "\r\n");
            else
            {
                thread.Stop = true;
                Application.Idle += new EventHandler(Capture_ImageGrabbed);
                this.listBox1.Items.Add(InvokerClass.String + "\r\n");
                
                //capture.ImageGrabbed += Capture_ImageGrabbed;
                capture.Start();

            }
        }

        /// <summary>
        /// 抓取图像事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            Mat frame = new Mat();
            AppendText("获取一个Mat"+ VideoFps1);
            if (capture == null || capture.Ptr == IntPtr.Zero)
                return;
            //将得到的图像检索到frame中
            capture.Retrieve(frame, 0);
            // frame = capture.QueryFrame();
            // System.Threading.Thread.Sleep((int)(1000.0 / VideoFps1 - 5));
            this.imageBox1.Image = frame;
            //System.Threading.Thread.Sleep((int)(1000.0 / VideoFps1 - 5));
            //_SyncContext.Send(o =>
            //{
            //    this.imageBox1.Image = frame;
            //}, null);
           // frame.Dispose();

        }

        public void aa()
        {
            //capture.ImageGrabbed += Capture_ImageGrabbed;
            this.listBox1.Items.Add("执行哈哈");

        }


        /// <summary>
        /// 追加公用方法
        /// </summary>
        /// <param name="message"></param>
        private void AppendText(string message)
        {
            this.listBox1.Items.Add(message);
        }

        private void Face_FormClosing(object sender, FormClosingEventArgs e)
        {
            thread.Stop = true;
        }

        private void Face_Load(object sender, EventArgs e)
        {
           // string RTSPStreamText = textBox1.Text.Trim();
            //capture = new Capture("rtmp://58.200.131.2:1935/livetv/dftv");
            //capture.ImageGrabbed += Capture_ImageGrabbed;
            //// capture.Pause();
            //capture.Start();
        }
    }
}
