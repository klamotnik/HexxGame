using Hexx.Server.Db;
using Hexx.Server.Output;
using Hexx.Server.Threads;
using Hexx.Server.Types;
using Hexx.Types;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Hexx.Server
{
    class Program
    {
        const int PORT_NO = 8960;
        const string SERVER_IP = "127.0.0.1";

        static void Main(string[] args)
        {
            ConsoleControl.ActivateControl();
            PrepareDatabaseConnection();
            
            PrepareThreads();
            ThreadManager threadManager = ThreadManager.GetInstance();
            threadManager.StartAllThreads();
            
            while (true)
            {
                LogBuffer.GetInstance().WriteBuffer();
                System.Threading.Thread.Sleep(100);
            }
        }

        private static void PrepareDatabaseConnection()
        {
            DatabaseConnectionManager manager = DatabaseConnectionManager.GetInstance();
            DatabaseConnectionData connectionData = new DatabaseConnectionData()
            {
                Address = ".\\SQLEXPRESS",
                DatabaseName = "Hexx",
                IntegratedLogin = true
            };
            manager.Initialize(connectionData);
            if (!manager.TestConnection())
                throw new Exception("Cannot connect to database " + connectionData.DatabaseName + ".");
        }

        private static void PrepareThreads()
        {
            ThreadManager threadManager = ThreadManager.GetInstance();
            LoopedThread connectionListener = new ConnectionListener(SERVER_IP, PORT_NO);
            LoopedThread requestListener = new RequestListener();
            LoopedThread notificationSender = new NotificationSender();
            threadManager.AddThread(connectionListener);
            threadManager.AddThread(requestListener);
            threadManager.AddThread(notificationSender);
        }
    }
}
