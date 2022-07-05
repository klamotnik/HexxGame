using Hexx.Components.Levels;
using Hexx.DTO;
using Hexx.Engine;
using Hexx.Engine.Types;
using Hexx.Types;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

namespace Hexx.Connection.Threads
{
    public class ResponseListener : LoopedThread
    {
        private TcpClient client;

        public ResponseListener(TcpClient client) : base()
        {
            this.client = client; 
            action = ResponseListenerThreadMethod;
            ThreadName = "ResponseListener";
        }

        public override void Stop()
        {
            base.Stop();
            client?.Close();
            client = null;
            //if (listenerThread != null && listenerThread.IsAlive)
            //    listenerThread.Abort();
        }

        private void ResponseListenerThreadMethod()
        {
            string responseData = "";
            while(!responseData.EndsWith("</Hexx>"))
                responseData += ReceiveDataFromServer(client);
            if (responseData == null)
                return;
            int index = responseData.IndexOf('<');
            if (index < 0)
                return;
            responseData = responseData.Substring(index);
            string[] splittedResponse = responseData.Split(new[] { "</Hexx>" }, StringSplitOptions.RemoveEmptyEntries).Select(p => p + "</Hexx>").ToArray();
            ConnectionBuffer buffer = ConnectionBuffer.GetInstance();
            foreach (string responsePart in splittedResponse)
            {
                try
                {
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(responsePart);
                    foreach (XmlDeclaration xmlDeclaration in document.ChildNodes.OfType<XmlDeclaration>())
                        document.RemoveChild(xmlDeclaration);
                    DTO.Objects.Hexx responseObj = (DTO.Objects.Hexx)Deserializer.Deserialize(document, typeof(DTO.Objects.Hexx));
                    IncomingMessageManager.PushMessage(responseObj.Action as Message);
                    buffer.PerformOperation(ConnectionBuffer.BufferOperation.Set, document.OuterXml);
                }
                catch (Exception)
                {

                }
            }
        }

        private string ReceiveDataFromServer(TcpClient tcpClient)
        {
            try
            {
                byte[] buffer = new byte[tcpClient.ReceiveBufferSize];
                NetworkStream stream = tcpClient.GetStream();
                if (stream.DataAvailable)
                {
                    int bytesRead = stream.Read(buffer, 0, tcpClient.ReceiveBufferSize);
                    stream.Flush();
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    return dataReceived;
                }
                else
                    Thread.Sleep(10);
                return null;
            }
            catch (Exception ex)
            {
                if(ex is InvalidOperationException)
                {
                    HandleServerConnectionLost();
                    Stop();
                }
                return null;
            }
        }

        private void HandleServerConnectionLost()
        {
            Level currentLevel = LevelManager.GetInstance().GetCurrentLevel();
            MessageBoxCallback callback = null;
            if (!(currentLevel is MainMenu))
            {
                callback = (result) => LevelManager.GetInstance().ChangeLevel<MainMenu>();
                Login.GetInstance().Logout();
            }
            MessageBoxService.GetInstance(currentLevel).Show("Connection with server has been lost.", MessageBoxButtons.OK, callback, true);
            ConnectionManager.GetInstance().Disconnect();
        }
    }
}
