using Hexx.DTO;
using Hexx.DTO.Notifications;
using Hexx.DTO.Objects;
using Hexx.DTO.Requests;
using Hexx.DTO.Responses;
using Hexx.Server.Db;
using Hexx.Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Server.Generators
{
    public class TableChangeNotificationGenerator : NotificationGenerator<TableChangeNotification>
    {
        private int tableNumber;
        private NotificationAction action;

        public TableChangeNotificationGenerator(Request request, Response response) : base(request, response)
        {
        }

        public override NotificationContainter.ServerNotification Generate()
        {
            if (!RetrieveData())
                return null;
            DatabaseContext context = new DatabaseContext();
            Db.Entity.Table table = context.Tables.SingleOrDefault(p=>p.Number == tableNumber);
            notification.Table = new Table()
            {
                Number = tableNumber
            };
            if (table == null/* || table.UsersOnTable.Count == 0*/)
                notification.Action = NotificationAction.Delete;
            else
            {
                table.LoadAllReferences(context);
                notification.Table.BoardSize = table.BoardSize;
                notification.Table.TimeForPlayer = table.TimeForPlayer;
                notification.Table.Seat1 = table.Player1?.Name;
                notification.Table.Seat2 = table.Player2?.Name;
            }

            return new NotificationContainter.ServerNotification(GetDestinationClients(), notification);
        }

        private bool RetrieveData()
        {
            ResponseStatus status;
            if (response is CreateTableResponse)
            {
                status = ((CreateTableResponse)response).Status;
                if (status != ResponseStatus.OK)
                    return false;
                tableNumber = ((CreateTableResponse)response).Table.Number;
                notification.Action = NotificationAction.New;
            }
            else if (request is LeaveTableRequest)
            {
                status = ((LeaveTableResponse)response).Status;
                if (status != ResponseStatus.OK)
                    return false;
                tableNumber = ((LeaveTableRequest)request).TableNumber;
                notification.Action = NotificationAction.Modify;
            }
            else if (request is TakeSeatRequest)
            {
                status = ((TakeSeatResponse)response).Status;
                if (status != ResponseStatus.OK)
                    return false;
                tableNumber = ((TakeSeatRequest)request).TableNumber;
                notification.Action = NotificationAction.Modify;
            }
            else if (request is LeaveSeatRequest)
            {
                status = ((LeaveSeatResponse)response).Status;
                if (status != ResponseStatus.OK)
                    return false;
                Db.Entity.UserOnTable userOnTable = context.UsersOnTable.Where(p => p.UserID == ((LeaveSeatRequest)request).Auth.UserID).FirstOrDefault();
                if (userOnTable == null)
                    return false;
                userOnTable.LoadAllReferences(context);
                tableNumber = userOnTable.Table.Number;
                notification.Action = NotificationAction.Modify;
            }
            else
                return false;
            return true;
        }

        private IEnumerable<ClientManager.Client> GetDestinationClients()
        {
            IEnumerable<string> clientGuids = context.UserTokens.Where(p=>p.User.CurrentTable == null || (notification.Action == NotificationAction.Modify && p.User.CurrentTable.Table.Number == notification.Table.Number)).Select(p=>p.ClientGuid);
            return ClientManager.GetInstance().GetActualClientList().Where(p => clientGuids.Contains(p.Guid.ToString("N")));
        }
    }
}
