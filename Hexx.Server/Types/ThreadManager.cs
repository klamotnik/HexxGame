using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hexx.Types;
using System.Threading;

namespace Hexx.Server.Types
{
    public class ThreadManager : Singleton<ThreadManager>
    {
        private List<object> threadList = new List<object>();

        public void AddThread(Thread thread)
        {
            threadList.Add(thread);
        }

        public void AddThread(LoopedThread thread)
        {
            threadList.Add(thread);
        }

        public void StartAllThreads()
        {
            foreach(object thread in threadList)
            {
                if (thread is Thread)
                    ((Thread)thread).Start();
                else if (thread is LoopedThread)
                    ((LoopedThread)thread).Start();
                else
                    throw new Exception("Unrecognized object type.");
            }
        }

        public void StopAllThreads()
        {
            foreach (object thread in threadList)
            {
                if (thread is Thread)
                    ((Thread)thread).Abort();
                else if (thread is LoopedThread)
                    ((LoopedThread)thread).Stop();
                else
                    throw new Exception("Unrecognized object type.");
            }
        }

        public void ForceStopAllThreads()
        {
            foreach (object thread in threadList)
            {
                if (thread is Thread)
                    ((Thread)thread).Abort();
                else if (thread is LoopedThread)
                    ((LoopedThread)thread).ForceStop();
                else
                    throw new Exception("Unrecognized object type.");
            }
        }

        public void StartThread(string threadName)
        {
            object thread = GetThread(threadName);
            if (thread is Thread)
                ((Thread)thread).Start();
            else
                ((LoopedThread)thread).Start();
        }

        public void StopThread(string threadName)
        {
            object thread = GetThread(threadName);
            if (thread is Thread)
                ((Thread)thread).Abort();
            else
                ((LoopedThread)thread).Stop();
        }

        public void ForceStopThread(string threadName)
        {
            object thread = GetThread(threadName);
            if (thread is Thread)
                ((Thread)thread).Abort();
            else
                ((LoopedThread)thread).ForceStop();
        }

        private object GetThread(string threadName)
        {
            object thread = threadList.OfType<Thread>().Where(p => p.Name == threadName).FirstOrDefault();
            if(thread == null)
                thread = threadList.OfType<LoopedThread>().Where(p => p.ThreadName == threadName).FirstOrDefault();
            if (thread == null)
                throw new Exception("Thread " + threadName + " not exists");
            return thread;
        }
    }
}
