using Hexx.Connection.Threads;
using Hexx.DTO;
using Hexx.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Hexx.Connection
{
    public class ConnectionManager : Singleton<ConnectionManager>
    {

        public const int DEFAULT_PORT = 8960;
        public const string DEFAULT_SERVER_IP = "127.0.0.1";//"89.74.39.212";
        private TcpClient client;
        private ResponseListener responseListenerThread;
        public bool IsConnected { get => client != null ? client.Connected : false; }

        public void Connect(string serverIP, int port)
        {
            if (client != null && client.Connected)
                throw new Exception("Connection exists. Disconnect current connection and try again.");
            if (client == null)
                client = new TcpClient();
            try
            {
                client.Connect(serverIP, port);
            }
            catch(ObjectDisposedException)
            {
                client = new TcpClient();
                client.Connect(serverIP, port);
            }
            if(responseListenerThread != null)
                responseListenerThread.ForceStop();
            responseListenerThread = new ResponseListener(client);
            responseListenerThread.Start();
        }

        public void Disconnect()
        {
            client?.Close();
            client = null;
            ResponseListener rl = responseListenerThread;
            responseListenerThread = null;
            rl?.ForceStop();
        }

        public bool Send(Request request)
        {
            DTO.Objects.Hexx hexx = request.WrapMessage();
            string requestMessage = Serializer.Serialize(hexx, hexx.GetType());
            BinaryWriter writer = new BinaryWriter(client.GetStream());
            try
            {
                writer.Write(new UTF8Encoding().GetBytes(requestMessage));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
