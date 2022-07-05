using CSSDL;
using CSSDL.Events;
using Hexx.Connection;
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

namespace Hexx.Components.Forms
{
    public class TablesListRow : ListRow, IInteractable
    {
        public bool CanInteract { get; set; }
        private Table tableInfo;
        private Label tableNumber;
        private Label gameTime;
        private Label seat1;
        private Label seat2;
        private Label tableSize;

        public TablesListRow(Table table) : base((0, 0, 460, 52), table.Number.ToString())
        {
            tableInfo = table;
            tableNumber = new Label((10, 10), 12, "#" + table.Number);
            gameTime = new Label(12, "Time: " + table.TimeForPlayer / 60 + ":" + (table.TimeForPlayer % 60 < 10 ? "0" : "") + table.TimeForPlayer % 60);
            gameTime.Refresh();
            gameTime.MoveTo(Viewport.Properties.w - gameTime.Viewport.Properties.w - 10, 10);
            tableSize = new Label((200, 10), 12, "Size: " + tableInfo.BoardSize);
            seat1 = new Label((10, 30), 12, table.Seat1 ?? "-");
            seat2 = new Label((230, 30), 12, table.Seat2 ?? "-");
            CanInteract = true;
            AddChild(tableNumber);
            AddChild(gameTime);
            AddChild(tableSize);
            AddChild(seat1);
            AddChild(seat2);
            EventManager.AddMouseButtonDownListener(MouseClick);
        }

        private void MouseClick(MouseButtonEvent e)
        {
            if (IsVisible && CanInteract && AbsolutePosition.IsPointInRectangle(e.X, e.Y))
            {
                Login login = Login.GetInstance();
                EnterTableRequest request = new EnterTableRequest();
                request.Auth = login.GetAuth();
                request.TableNumber = tableInfo.Number;
                IncomingMessageManager.AddIncomingMessageListener<EnterTableResponse>(EnterTableCallback);
                ConnectionManager.GetInstance().Send(request);
            }
        }

        private void EnterTableCallback(EnterTableResponse response)
        {
            IncomingMessageManager.RemoveIncomingMessageListener<EnterTableResponse>(EnterTableCallback);
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
            Viewport.Fill(Engine.Types.Color.White);
            DrawingProcessor dp = new DrawingProcessor(Viewport.Surface);
            dp.DrawLine(0, Viewport.Properties.h - 1, Viewport.Properties.w - 1, Viewport.Properties.h - 1, 1, CSSDL.Color.Black);
            foreach (Actor child in childrens)
            {
                if (child.NeedRefresh)
                    child.Refresh();
                Viewport.Draw(child);
            }
        }
    }
}
