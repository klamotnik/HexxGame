using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

namespace Hexx.Connection
{
    public class ListenerThread
    {
        private Thread listenerThread;
        private TcpClient client;

        public ListenerThread(TcpClient client)
        {
            this.client = client;
        }

        public void Run()
        {
            listenerThread = new Thread(Action);
            listenerThread.Start();
        }

        private void Action()
        {
            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            NetworkStream stream = client.GetStream();
            string tekst;
            try
            {
                while (stream.Read(bytesToRead, 0, client.ReceiveBufferSize) > 0 && !string.IsNullOrEmpty(tekst = Encoding.UTF8.GetString(bytesToRead)))
                {
                    string response = tekst.Replace("\0", "").Replace("\r\n", "");
                    if (!string.IsNullOrEmpty(response))
                    {
                        string[] splittedResponse = response.Split(new[] { "</Hexx>" }, StringSplitOptions.RemoveEmptyEntries).Select(p => p + "</Hexx>").ToArray();
                        ConnectionBuffer buffer = ConnectionBuffer.GetInstance();
                        foreach (string responsePart in splittedResponse)
                        {
                            try
                            {
                                XmlDocument document = new XmlDocument();
                                document.LoadXml(responsePart);
                                foreach (XmlDeclaration xmlDeclaration in document.ChildNodes.OfType<XmlDeclaration>())
                                    document.RemoveChild(xmlDeclaration);
                                DTO.Objects.Hexx responseObj = (DTO.Objects.Hexx)DTO.Deserializer.Deserialize(document, typeof(DTO.Objects.Hexx));
                                buffer.PerformOperation(ConnectionBuffer.BufferOperation.Set, document.OuterXml);
                            }
                            catch(Exception ex)
                            {

                            }
                        }
                    }
                    bytesToRead = new byte[client.ReceiveBufferSize];
                }
            }
            catch { }
            if (client != null)
                client.Close();
        }

        public void Stop()
        {
            client.Close();
            if (listenerThread != null && listenerThread.IsAlive)
                listenerThread.Abort();
        }
    }
}
