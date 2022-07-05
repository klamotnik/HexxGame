using CSSDL;
using Hexx.Components.Levels;
using Hexx.Connection;
using Hexx.DTO;
using Hexx.DTO.Objects;
using Hexx.DTO.Requests;
using Hexx.DTO.Responses;
using Hexx.Engine;
using Hexx.Engine.Types;
using System;
using System.Linq;

namespace Hexx.Components
{
    public class RoomLoginBar : Actor
    {
        private Label loggedInAs;
        private Button logoutButton;
        private Button newTableButton;

        public RoomLoginBar(Rectangle rectangle) : base(rectangle)
        {
            Login login = Login.GetInstance();
            loggedInAs = new Label((10, 7), 16, "Logged in as: " + login.Username);
            loggedInAs.FontColor = Engine.Types.Color.White;
            logoutButton = new Button((rectangle.w - 80, 5, 70, 20), "Logout", "Logout", Logout);
            newTableButton = new Button((logoutButton.Viewport.Properties.x - 110, 5, 100, 20), "NewTable", "New table", CreateTable);
            AddChild(loggedInAs);
            AddChild(logoutButton);
            AddChild(newTableButton);
        }

        private void Logout()
        {
            Login login = Login.GetInstance();
            LogoutRequest request = new LogoutRequest();
            request.Auth = login.GetAuth();
            IncomingMessageManager.AddIncomingMessageListener<LogoutResponse>(LogoutCallback);
            ConnectionManager.GetInstance().Send(request);
        }

        private void LogoutCallback(LogoutResponse response)
        {
            IncomingMessageManager.RemoveIncomingMessageListener<LogoutResponse>(LogoutCallback);
            ResponseProcessor processor = ResponseProcessor.GetProcessor(response);
            processor.Process();
        }

        private void CreateTable()
        {
            Login login = Login.GetInstance();
            CreateTableRequest request = new CreateTableRequest();
            request.Auth = login.GetAuth();
            request.Table = new Table()
            {
                TimeForPlayer = 300,
                BoardSize = 5
            };
            IncomingMessageManager.AddIncomingMessageListener<CreateTableResponse>(CreateTableCallback);
            ConnectionManager.GetInstance().Send(request);
        }

        private void CreateTableCallback(Response response)
        {
            IncomingMessageManager.RemoveIncomingMessageListener<CreateTableResponse>(CreateTableCallback);
            ResponseProcessor processor = ResponseProcessor.GetProcessor(response);
            processor.Process();
        }

        public override void Refresh()
        {
            base.Refresh();
            if (!IsVisible)
                return;
            if (childrens.Any(p => p.NeedRefresh))
                Viewport.Clear();
            DrawingProcessor dp = new DrawingProcessor(Viewport.Surface);
            dp.DrawRoundedBorder(new CSSDL.Color() { r = 255, g = 255, b = 255, a = 0xAA });
            dp.FillPolygon(2, 4, new CSSDL.Color() { r = 255, g = 255, b = 255, a = 0xAA });
            foreach (Actor child in childrens)
            {
                if (child.NeedRefresh)
                    child.Refresh();
                Viewport.Draw(child);
            }
        }
    }
}
