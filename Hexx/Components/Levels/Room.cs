using CSSDL;
using Hexx.Components.Forms;
using Hexx.Components.Panels;
using Hexx.Connection;
using Hexx.DTO;
using Hexx.DTO.Notifications;
using Hexx.DTO.Objects;
using Hexx.DTO.Requests;
using Hexx.DTO.Responses;
using Hexx.Engine;
using Hexx.Engine.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Components.Levels
{
    public class Room : Level
    {
        private List tablesList;
        private List playersList;
        private RoomLoginBar loginBar;

        public Room(Viewport viewport) : base(viewport)
        {
            tablesList = new List((10, 50, 460, 420));
            playersList = new List((480, 50, 150, 420));
            loginBar = new RoomLoginBar((10, 10, 620, 30));
            AddActor(tablesList);
            AddActor(playersList);
            AddActor(loginBar);
            IncomingMessageManager.AddIncomingMessageListener<PlayerChangeNotification>(PlayerChange);
            IncomingMessageManager.AddIncomingMessageListener<TableChangeNotification>(TableChange);
            RequestForRoomInfo();
        }

        public override void Dispose()
        {
            IncomingMessageManager.RemoveIncomingMessageListener<PlayerChangeNotification>(PlayerChange);
            IncomingMessageManager.RemoveIncomingMessageListener<TableChangeNotification>(TableChange);
        }

        private void RequestForRoomInfo()
        {
            Login login = Login.GetInstance();
            GetRoomInfoRequest request = new GetRoomInfoRequest();
            request.Auth = login.GetAuth();
            IncomingMessageManager.AddIncomingMessageListener<GetRoomInfoResponse>(GetRoomInfoCallback);
            ConnectionManager.GetInstance().Send(request);
        }

        private void PlayerChange(PlayerChangeNotification response)
        {
            if (response.Player.Name == Login.GetInstance().Username)
                return;
            if (response.Action == NotificationAction.New)
                playersList.AddChild(new PlayersListRow(response.Player));
            else if (response.Action == NotificationAction.Delete)
                playersList.RemoveChild(response.Player.Name);
        }

        private void TableChange(TableChangeNotification response)
        {
            if (response.Action == NotificationAction.New)
                tablesList.AddChild(new TablesListRow(response.Table));
            else if (response.Action == NotificationAction.Delete)
                tablesList.RemoveChild(response.Table.Number.ToString());
            else if (response.Action == NotificationAction.Modify)
            {
                tablesList.RemoveChild(response.Table.Number.ToString());
                tablesList.AddChild(new TablesListRow(response.Table));
            }
        }

        private void GetRoomInfoCallback(GetRoomInfoResponse response)
        {
            IncomingMessageManager.RemoveIncomingMessageListener<GetRoomInfoResponse>(GetRoomInfoCallback);
            ResponseProcessor processor = ResponseProcessor.GetProcessor(response);
            processor.Process();
            if (response.Status == ResponseStatus.OK)
            {
                if (response.RoomInfo.Tables.Table != null)
                    foreach (Table table in response.RoomInfo.Tables.Table)
                        tablesList.AddChild(new TablesListRow(table));
                if (response.RoomInfo.Players.Player != null)
                    foreach (Player player in response.RoomInfo.Players.Player)
                        playersList.AddChild(new PlayersListRow(player));
            }
        }
    }
}
