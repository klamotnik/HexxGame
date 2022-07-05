using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hexx.DTO;
using Hexx.DTO.Notifications;
using Hexx.DTO.Objects;
using Hexx.Server.Db;

namespace Hexx.Server.Types
{
    public abstract class NotificationGenerator<T> : NotificationGenerator where T : Notification
    {
        protected T notification;

        public NotificationGenerator(Request request, Response response) : base(request, response)
        {
            notification = (T)Activator.CreateInstance(typeof(T));
        }
    }

    public abstract class NotificationGenerator
    {
        protected Request request;
        protected Response response;

        protected DatabaseContext context;
        public NotificationGenerator(Request request, Response response)
        {
            this.request = request;
            this.response = response;
            DatabaseConnectionManager connectionManager = DatabaseConnectionManager.GetInstance();
            context = new DatabaseContext();
        }

        public abstract NotificationContainter.ServerNotification Generate();
    }
}
