using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Hexx.Server.Types;
using Hexx.Server.Output;
using Hexx.Types;
using Hexx.Server.Db;
using Hexx.Server.Db.Entity;

namespace Hexx.Server.Threads
{
    public class ConnectionListener : LoopedThread
    {
        private LogBuffer logBuffer;
        private ClientManager connectionManager;
        private TcpListener tcpListener;
        private string address;
        private int port;

        public ConnectionListener(string address, int port) : base()
        {
            this.address = address;
            this.port = port;
            action = ConnectionListenerThreadMethod;
            ThreadName = "ConnectionListener";
            logBuffer = LogBuffer.GetInstance();
            connectionManager = ClientManager.GetInstance();
        }

        public override void Start()
        {
            IPAddress localAdd = IPAddress.Parse(address);
            tcpListener = new TcpListener(localAdd, port);
            tcpListener.Start();
            logBuffer.Push("Listening...");
            base.Start();
        }

        public override void Stop()
        {
            tcpListener.Stop();
            base.Stop();
        }

        private void ConnectionListenerThreadMethod()
        {
            if (tcpListener.Pending())
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                connectionManager.AddClient(client);
                logBuffer.Push("Client has been connected");
            }
            foreach (ClientManager.Client client in connectionManager.GetActualClientList())
                if (ClientManager.IsClientDisconnected(client))
                    RemoveClient(client.TcpClient);
            Thread.Sleep(100);
        }

        private void RemoveClient(TcpClient tcpClient)
        {
            LogoutUser(tcpClient);
            connectionManager.RemoveClient(tcpClient);
            try
            {
                tcpClient.Client.Disconnect(false);
                tcpClient.Close();
            }
            catch { }
            logBuffer.Push("Client has been closed");
        }

        private void LogoutUser(TcpClient tcpClient)
        {
            string clientGuid = connectionManager.GetClientGuid(tcpClient);
            if (clientGuid == null)
                return;
            DatabaseContext context = new DatabaseContext();
            UserToken userToken = context.UserTokens.SingleOrDefault(p=>p.ClientGuid == clientGuid);
            if (userToken != null)
            {
                UserOnTable userOnTable = context.UsersOnTable.SingleOrDefault(p => p.UserID == userToken.ID);
                if (userOnTable != null)
                {
                    userOnTable.LoadAllReferences(context);
                    if (userOnTable.Table.Seat1 == userToken.ID)
                        userOnTable.Table.Seat1 = 0;
                    if (userOnTable.Table.Seat2 == userToken.ID)
                        userOnTable.Table.Seat2 = 0;
                    if (context.UsersOnTable.Count(p=>p.Table == userOnTable.Table) == 1)
                        context.Remove(userOnTable.Table);// + tablechangenotification
                    context.Remove(userOnTable);
                }
                context.Remove(userToken);
                context.SaveChanges();
                //push playerchangenotification
            }
        }
    }
}