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
    public class RequestListener : LoopedThread
    {
        private LogBuffer logBuffer;

        public RequestListener() : base()
        {
            action = RequestListenerThreadMethod;
            ThreadName = "RequestListener";
            logBuffer = LogBuffer.GetInstance();
        }

        private void RequestListenerThreadMethod()
        {
            ClientManager clientManager = ClientManager.GetInstance();
            IEnumerable<ClientManager.Client> clients = clientManager.GetActualClientList();
            foreach (ClientManager.Client client in clients)
            {
                string dataReceived = ReceiveDataFromClient(client);
                if (string.IsNullOrEmpty(dataReceived))
                    continue;
                int index = dataReceived.IndexOf('<');
                if (index < 0)
                    return;
                dataReceived = dataReceived.Substring(index);
                string[] splittedRequest = dataReceived.Split(new[] { "</Hexx>" }, StringSplitOptions.RemoveEmptyEntries).Select(p => p + "</Hexx>").ToArray();
                foreach (string responsePart in splittedRequest)
                {
                    logBuffer.Push("Received request from " + client.Guid.ToString("N"));
                    DTO.Objects.Hexx response = ProcessRequestAndPrepareResponse(client, responsePart);
                    if (response == null)
                    {
                        logBuffer.Push("Data received from client " + client.Guid.ToString("N") + " is damaged.");
                        continue;
                    }
                    logBuffer.Push("Sending response to " + client.Guid.ToString("N"));
                    SendResponseToClient(client, response);
                }
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
                if (stream.DataAvailable)
                {
                    int bytesRead = stream.Read(buffer, 0, tcpClient.ReceiveBufferSize);
                    stream.Flush();
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    return dataReceived;
                }
                return null;
            }
            catch (Exception)
            {
                //try
                //{
                //    client.TcpClient.GetStream().Flush();
                //}
                //catch (InvalidOperationException)
                //{
                //    //force delete client
                //}
                return null;
            }
        }

        private DTO.Objects.Hexx ProcessRequestAndPrepareResponse(ClientManager.Client client, string dataReceived)
        {
            DTO.Objects.Hexx request = null;
            try
            {
                request = Deserializer.Deserialize(dataReceived, typeof(DTO.Objects.Hexx)) as DTO.Objects.Hexx;
            }
            catch (Exception) // some kind of shit in received data
            {
                return null;
            }
            RequestProcessor requestProcessor = RequestProcessorFactory.GetProcessor(request, client);
            Response response = requestProcessor.Process();
            foreach (NotificationGenerator generator in NotificationGeneratorFactory.GetGeneratorsByRequestResponse(request.Action as Request, response))
            {
                NotificationContainter.ServerNotification notification = generator.Generate();
                if (notification != null)
                    NotificationContainter.GetInstance().AddNotification(notification);
            }
            DTO.Objects.Hexx serverResponse = WrapResponse(response);
            return serverResponse;
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
