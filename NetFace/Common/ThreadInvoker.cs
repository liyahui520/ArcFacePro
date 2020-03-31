using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetFace.Common
{
    public class ThreadInvoker
    {
        /// <summary>
        /// 回调委托 带参数的
        /// </summary>
        /// <param name="InvokerClass"></param>
        public delegate void CallbackFunc(InvokerClass InvokerClass);
        /// <summary>
        /// 回调委托的方法
        /// </summary>
        public CallbackFunc AsynCallback;
        /// <summary>
        /// 线程
        /// </summary>
        public Thread thread;
        /// <summary>
        /// 执行循环停止属性
        /// </summary>
        public bool Stop = false;
        /// <summary>
        /// 休眠间隔
        /// </summary>
        public int Sleep = 1000;
        public ThreadInvoker(CallbackFunc callback)
        {
            AsynCallback = callback;
        }
        public virtual void Start(ThreadStart ThreadStart)
        {
            thread = new Thread(ThreadStart);
            thread.Start();
        }

        public virtual void Start(ParameterizedThreadStart ThreadStart)
        {
            thread = new Thread(ThreadStart);
            thread.Start();
        }
    }

    public class InvokerClass
    {
        public string String { get; set; }

    }
}
