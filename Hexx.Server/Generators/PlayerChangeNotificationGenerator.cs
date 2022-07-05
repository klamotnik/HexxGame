using Hexx.DTO;
using Hexx.DTO.Notifications;
using Hexx.DTO.Objects;
using Hexx.DTO.Requests;
using Hexx.DTO.Responses;
using Hexx.Server.Db;
using Hexx.Server.Db.Entity;
using Hexx.Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Server.Generators
{
    public class PlayerChangeNotificationGenerator : NotificationGenerator<PlayerChangeNotification>
    {
        private int userID;
        private NotificationAction action;

        public PlayerChangeNotificationGenerator(Request request, Response response) : base(request, response)
        {
        }

        public override NotificationContainter.ServerNotification Generate()
        {
            if (!RetrieveData())
                return null;
            User user = context.Users.SingleOrDefault(p => p.ID == userID);
            user.LoadAllReferences(context);
            user.CurrentTable?.LoadAllReferences(context);
            notification.Player = new Player()
            {
                Name = user.Name,
                Rank = user.UserData.Rank,
                Seat = user.CurrentTable?.Table.Seat1 == userID ? 1 : user.CurrentTable?.Table.Seat2 == userID ? 2 : 0,
                Table = user.CurrentTable?.TableID ?? 0
            };
            return new NotificationContainter.ServerNotification(GetDestinationClients(), notification);
        }

        private bool RetrieveData()
        {
            ResponseStatus status;
            if (response is LogoutResponse)
            {
                status = ((LogoutResponse)response).Status;
                if (status != ResponseStatus.OK)
                    return false;
                userID = ((LogoutRequest)request).Auth.UserID;
                notification.Action = NotificationAction.Delete;
            }
            else if (request is LoginRequest)
            {
                status = ((LoginResponse)response).Status;
                if (status != ResponseStatus.OK)
                    return false;
                userID = ((LoginResponse)response).Auth.UserID;
                notification.Action = NotificationAction.New;
            }
            else
                return false;
            return true;
        }

        private IEnumerable<ClientManager.Client> GetDestinationClients()
        {
            IEnumerable<string> clientGuids = context.UserTokens.Where(p=>p.User.CurrentTable == null).Select(p=>p.ClientGuid);
            return ClientManager.GetInstance().GetActualClientList().Where(p => clientGuids.Contains(p.Guid.ToString("N")));
        }
    }
}
