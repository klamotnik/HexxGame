using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Hexx.Types;

namespace Hexx.Server.Types
{
    public sealed class ClientManager : Singleton<ClientManager>
    {
        public class Client
        {
            public TcpClient TcpClient { get; private set; }
            public Guid Guid { get; private set; }

            public Client(TcpClient tcpClient, Guid guid)
            {
                TcpClient = tcpClient;
                Guid = guid;
            }
        }

        private List<Client> clientList = new List<Client>();

        public static bool IsClientDisconnected(Client client)
        {
            return IsClientDisconnected(client.TcpClient);
        }

        public static bool IsClientDisconnected(TcpClient tcpClient)
        {
            try
            {
                if (tcpClient.Client.Poll(0, SelectMode.SelectRead))
                {
                    byte[] buff = new byte[1];
                    if (tcpClient.Client.Receive(buff, SocketFlags.Peek) == 0)
                        return true;
                }
            }
            catch (SocketException)
            {
                return true;
            }
            return false;
        }

        public bool AddClient(TcpClient tcpClient)
        {
            if (!clientList.Any(p => p.TcpClient == tcpClient))
            {
                Client client = new Client(tcpClient, GetUniqueGuid());
                lock (clientList)
                    clientList.Add(client);
                return true;
            }
            return false;
        }

        public bool RemoveClient(TcpClient tcpClient)
        {
            Client client = clientList.Where(p => p.TcpClient == tcpClient).FirstOrDefault();
            if (client != null)
            {
                lock (clientList)
                    clientList.Remove(client);
                return true;
            }
            return false;
        }

        public IEnumerable<Client> GetActualClientList()
        {
            lock (clientList)
                return clientList.ToArray();
        }

        public string GetClientGuid(TcpClient tcpClient)
        {
            return clientList.Where(p => p.TcpClient == tcpClient).FirstOrDefault()?.Guid.ToString("N");
        }

        private Guid GetUniqueGuid()
        {
            Guid guid = Guid.NewGuid();
            while (clientList.Any(p => p.Guid == guid))
                guid = Guid.NewGuid();
            return guid;
        }
    }
}
