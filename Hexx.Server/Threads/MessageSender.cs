using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Hexx.DTO;
using Hexx.Server.Output;
using Hexx.Server.Types;
using Hexx.Types;

namespace Hexx.Server.Threads
{
    public class MessageSender : LoopedThread
    {
        private LogBuffer logBuffer;

        public MessageSender() : base()
        {
            action = MessageSenderThreadMethod;
            ThreadName = "MessageSender";
            logBuffer = LogBuffer.GetInstance();
        }

        private void MessageSenderThreadMethod()
        {
            ResponseContainer responseContainer = ResponseContainer.GetInstance();
            IEnumerable<ResponseContainer.ServerResponse> serverResponses = responseContainer.GetPendingResponses();
            foreach (ResponseContainer.ServerResponse serverResponse in serverResponses)
            {
                DTO.Objects.Hexx wrappedResponse = WrapResponse(serverResponse.Response);
                SendResponseToClient(serverResponse.Client, wrappedResponse);
                //GenerateNotification();
            }
            Thread.Sleep(100);
        }

        private string ReceiveDataFromClient(ClientManager.Client client)
        {
            try
            {
                TcpClient tcpClient = client.TcpClient;
                byte[] buffer = new byte[tcpClient.ReceiveBufferSize];
                NetworkStream stream = tcpClient.GetStream();
                int bytesRead = stream.Read(buffer, 0, tcpClient.ReceiveBufferSize);
                stream.Flush();
                string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                return dataReceived;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        private DTO.Objects.Hexx WrapResponse(Response response)
        {
            DTO.Objects.Hexx serverResponse = new DTO.Objects.Hexx()
            {
                Action = response,
                Type = (DTO.Objects.ActionType)Enum.Parse(typeof(DTO.Objects.ActionType), response.GetType().Name.ToString())
            };
            return serverResponse;
        }

        private void SendResponseToClient(ClientManager.Client client, DTO.Objects.Hexx response)
        {
            if (!(response.Action is Response))
                throw new Exception("There is no response to send to client");
            if (ClientManager.IsClientDisconnected(client))
                return;
            string responseText = string.Empty;
            try
            {
                responseText = Serializer.Serialize(response, typeof(DTO.Objects.Hexx));
                BinaryWriter writer = new BinaryWriter(client.TcpClient.GetStream());
                writer.Flush();
                writer.Write(responseText);
                writer.Flush();
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
