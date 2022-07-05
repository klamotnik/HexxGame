using System.Collections.Generic;
using Hexx.Types;
using Hexx.DTO;

namespace Hexx.Server.Types
{
    public class NotificationContainter : Singleton<NotificationContainter>
    {
        public class ServerNotification
        {
            public IEnumerable<ClientManager.Client> Clients { get; private set; }
            public Notification Notification { get; private set; }

            public ServerNotification(IEnumerable<ClientManager.Client> clients, Notification notification)
            {
                Clients = clients;
                Notification = notification;
            }
        }

        private List<ServerNotification> serverNotificationList = new List<ServerNotification>();

        public void AddNotification(ServerNotification notification)
        {
            lock (serverNotificationList)
                serverNotificationList.Add(notification);
        }

        public void AddResponse(ClientManager.Client[] clients, Notification notification)
        {
            ServerNotification serverResponse = new ServerNotification(clients, notification);
            AddNotification(serverResponse);
        }

        public IEnumerable<ServerNotification> GetPendingNotifications()
        {
            ServerNotification[] serverNotifications;
            lock (serverNotificationList)
            {
                serverNotifications = serverNotificationList.ToArray();
                serverNotificationList.Clear();
            }
            return serverNotifications;
        }
    }
}
