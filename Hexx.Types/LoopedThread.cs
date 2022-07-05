using System;
using System.Threading;
using Hexx.Types.Interfaces;

namespace Hexx.Types
{
    public delegate void ThreadAction();

    public abstract class LoopedThread : IThread
    {
        private bool canProcess;
        private Thread thread;
        protected ThreadAction action;
        public string ThreadName { get; set; }

        public virtual void Start()
        {
            if (thread?.ThreadState == ThreadState.Running || thread?.ThreadState == ThreadState.WaitSleepJoin)
                throw new Exception("Thread is running.");
            canProcess = true;
            thread = new Thread(ThreadMethod);
            thread.Name = ThreadName;
            thread.Start();
        }

        public virtual void Stop()
        {
            canProcess = false;
        }

        public virtual void ForceStop()
        {
            Stop();
            thread.Abort();
        }

        private void ThreadMethod()
        {
            while (canProcess)
                action();
        }
    }
}
