using CSSDL;
using Hexx.Components.Levels;
using Hexx.Connection;
using Hexx.DTO;
using Hexx.DTO.Notifications;
using Hexx.DTO.Objects;
using Hexx.DTO.Requests;
using Hexx.DTO.Responses;
using Hexx.Engine;
using Hexx.Engine.Types;
using System;
using System.Linq;

namespace Hexx.Components
{
    public class GameInfoBar : Actor
    {
        private Table tableInfo;
        private Label tableNumber;
        private Label seat1Name;
        private Label seat1Points;
        private Label seat1Time;
        private Label seat2Name;
        private Label seat2Points;
        private Label seat2Time;
        private Button takeSeat1Button;
        private Button takeSeat2Button;
        private Button leaveSeat1Button;
        private Button leaveSeat2Button;

        public GameInfoBar(Rectangle rectangle) : base(rectangle)
        {
            Login login = Login.GetInstance();
            tableInfo = new Table() { Number = login.CurrentTable.Number, Seat1 = login.CurrentTable.Seat1, Seat2 = login.CurrentTable.Seat2 };
            Viewport = new Viewport((0, 0, 550, 72));
            Viewport.MoveTo(rectangle.x, rectangle.y);
            tableNumber = new Label((486, 29), 12, "#" + tableInfo.Number);
            tableNumber.FontColor = Engine.Types.Color.White;
            seat1Name = new Label((57, 33, 163, 30), 16, tableInfo.Seat1);
            seat1Points = new Label((13, 33, 43, 30), 16, "0");
            seat1Time = new Label((96, 12, 43, 30), 16, "7:30");
            seat2Name = new Label((263, 33, 163, 30), 16, tableInfo.Seat2);
            seat2Points = new Label((427, 33, 43, 30), 16, "0");
            seat2Time = new Label((348, 12, 43, 30), 16, "7:30");
            leaveSeat1Button = new Button((199, 38, 16, 20), "LeaveSeat1", "X", LeaveSeat);
            leaveSeat1Button.FontSize = 12;
            leaveSeat2Button = new Button((405, 38, 16, 20), "LeaveSeat2", "X", LeaveSeat);
            leaveSeat2Button.FontSize = 12;
            takeSeat1Button = new Button((94, 38, 89, 20), "TakeSeat1", "Take seat", TakeSeat1);
            takeSeat1Button.FontSize = 12;
            takeSeat2Button = new Button((300, 38, 89, 20), "TakeSeat2", "Take seat", TakeSeat2);
            takeSeat2Button.FontSize = 12;
            RefreshButtons();

            AddChild(tableNumber);
            AddChild(seat1Name);
            AddChild(seat1Points);
            AddChild(seat1Time);
            AddChild(seat2Name);
            AddChild(seat2Points);
            AddChild(seat2Time);
            AddChild(leaveSeat1Button);
            AddChild(leaveSeat2Button);
            AddChild(takeSeat1Button);
            AddChild(takeSeat2Button);
            NeedRefresh = true;

            IncomingMessageManager.AddIncomingMessageListener<TableChangeNotification>(TableInfoChange);
        }

        public void RefreshButtons()
        {
            try
            {
                Login login = Login.GetInstance();
                leaveSeat1Button.IsVisible = tableInfo.Seat1 == login.Username;
                leaveSeat2Button.IsVisible = tableInfo.Seat2 == login.Username;
                takeSeat1Button.IsVisible = string.IsNullOrEmpty(tableInfo.Seat1) && tableInfo.Seat2 != login.Username;
                takeSeat2Button.IsVisible = string.IsNullOrEmpty(tableInfo.Seat2) && tableInfo.Seat1 != login.Username;
            }
            catch
            {

            }
        }

        public void UpdatePoints(int p1, int p2)
        {
            seat1Points.Text = p1.ToString();
            seat2Points.Text = p2.ToString();
        }

        private void TakeSeat1()
        {
            Login login = Login.GetInstance();
            TakeSeatRequest request = new TakeSeatRequest();
            request.Auth = login.GetAuth();
            request.TableNumber = login.CurrentTable.Number;
            request.Seat = 1;
            IncomingMessageManager.AddIncomingMessageListener<TakeSeatResponse>(TakeSeat1Callback);
            ConnectionManager.GetInstance().Send(request);
        }

        private void TakeSeat1Callback(TakeSeatResponse response)
        {
            IncomingMessageManager.RemoveIncomingMessageListener<TakeSeatResponse>(TakeSeat1Callback);
            ResponseProcessor processor = ResponseProcessor.GetProcessor(response);
            processor.Process();
        }

        private void TakeSeat2()
        {
            Login login = Login.GetInstance();
            TakeSeatRequest request = new TakeSeatRequest();
            request.Auth = login.GetAuth();
            request.TableNumber = login.CurrentTable.Number;
            request.Seat = 2;
            IncomingMessageManager.AddIncomingMessageListener<TakeSeatResponse>(TakeSeat2Callback);
            ConnectionManager.GetInstance().Send(request);
        }

        private void TakeSeat2Callback(TakeSeatResponse response)
        {
            IncomingMessageManager.RemoveIncomingMessageListener<TakeSeatResponse>(TakeSeat2Callback);
            ResponseProcessor processor = ResponseProcessor.GetProcessor(response);
            processor.Process();
        }

        private void LeaveSeat()
        {
            Login login = Login.GetInstance();
            LeaveSeatRequest request = new LeaveSeatRequest();
            request.Auth = login.GetAuth();
            IncomingMessageManager.AddIncomingMessageListener<LeaveSeatResponse>(LeaveSeatCallback);
            ConnectionManager.GetInstance().Send(request);
        }

        private void LeaveSeatCallback(LeaveSeatResponse response)
        {
            IncomingMessageManager.RemoveIncomingMessageListener<LeaveSeatResponse>(LeaveSeatCallback);
            ResponseProcessor processor = ResponseProcessor.GetProcessor(response);
            processor.Process();
        }

        private void TableInfoChange(TableChangeNotification response)
        {
            if (response.Action == NotificationAction.Modify && response.Table.Number == tableInfo.Number)
            {
                tableInfo.Seat1 = response.Table.Seat1;
                tableInfo.Seat2 = response.Table.Seat2;
                seat1Name.Text = response.Table.Seat1;
                seat2Name.Text = response.Table.Seat2;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            if (!IsVisible)
                return;
            if (childrens.Any(p => p.NeedRefresh))
                Viewport.Clear();
            DrawingProcessor dp = new DrawingProcessor(Viewport.Surface);
            dp.DrawRectangle((0, 0, Viewport.Properties.w, Viewport.Properties.h), 1, new CSSDL.Color() { r = 0, g = 0, b = 0, a = 0xAA });

            dp.FillPolygon(2, 4, new CSSDL.Color() { r = 0, g = 0, b = 0, a = 0xAA });
            dp.DrawRectangle((11, 11, 209, 52), 1, CSSDL.Color.Black);
            dp.FillPolygon(13, 13, CSSDL.Color.White);
            dp.DrawRectangle((11, 11, 209, 21), 1, CSSDL.Color.Black);
            dp.FillPolygon(12, 12, CSSDL.Color.Red);
            dp.DrawRectangle((11, 31, 45, 32), 1, CSSDL.Color.Black);

            dp.DrawRectangle((262, 11, 209, 52), 1, CSSDL.Color.Black);
            dp.FillPolygon(263, 12, CSSDL.Color.White);
            dp.DrawRectangle((262, 11, 209, 21), 1, CSSDL.Color.Black);
            dp.FillPolygon(263, 12, CSSDL.Color.Blue);
            dp.DrawRectangle((426, 31, 45, 32), 1, CSSDL.Color.Black);
            foreach (Actor child in childrens)
            {
                if (child.NeedRefresh)
                    child.Refresh();
                Viewport.Draw(child);
            }
            RefreshButtons();
        }
    }
}
