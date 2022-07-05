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
    public class NotificationSender : LoopedThread
    {
        private LogBuffer logBuffer;

        public NotificationSender() : base()
        {
            action = NotificationSenderThreadMethod;
            ThreadName = "NotificationSender";
            logBuffer = LogBuffer.GetInstance();
        }

        private void NotificationSenderThreadMethod()
        {
            NotificationContainter notificationContainter = NotificationContainter.GetInstance();
            IEnumerable<NotificationContainter.ServerNotification> serverNotifications = notificationContainter.GetPendingNotifications();
            foreach (NotificationContainter.ServerNotification serverNotification in serverNotifications)
            {
                DTO.Objects.Hexx wrappedResponse = WrapResponse(serverNotification.Notification);
                foreach(ClientManager.Client client in serverNotification.Clients)
                    SendResponseToClient(client, wrappedResponse);
            }
            Thread.Sleep(100);
        }
        
        private DTO.Objects.Hexx WrapResponse(Notification notification)
        {
            DTO.Objects.Hexx serverResponse = new DTO.Objects.Hexx()
            {
                Action = notification,
                Type = (DTO.Objects.ActionType)Enum.Parse(typeof(DTO.Objects.ActionType), notification.GetType().Name.ToString())
            };
            return serverResponse;
        }

        private void SendResponseToClient(ClientManager.Client client, DTO.Objects.Hexx notification)
        {
            if (!(notification.Action is Notification))
                throw new Exception("There is no notification to send to client");
            if (ClientManager.IsClientDisconnected(client))
                return;
            string responseText = string.Empty;
            try
            {
                responseText = Serializer.Serialize(notification, typeof(DTO.Objects.Hexx));
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
