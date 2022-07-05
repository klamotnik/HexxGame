using Hexx.Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Server
{
    public static class ConsoleControl
    {
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        private static EventHandler handler;

        private enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig)
        {
            StopServer();
            return false;
        }

        private static void StopServer()
        {
            ThreadManager threadManager = ThreadManager.GetInstance();
            threadManager.ForceStopAllThreads();
        }

        public static void ActivateControl()
        {
            if(handler == null)
                handler = new EventHandler(Handler);
            SetConsoleCtrlHandler(handler, true);
        }

        public static void DisableControl()
        {
            if (handler == null)
                return;
            SetConsoleCtrlHandler(handler, false);
        }
    }
}
