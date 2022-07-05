using CSSDL;
using Hexx.Components.Forms;
using Hexx.Components.Panels;
using Hexx.Connection;
using Hexx.DTO;
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
    public class GameTable : Level
    {
        private Button leaveTableButton;
        private GameInfoBar gameInfoBar;

        public GameTable(Viewport viewport) : base(viewport)
        {
            leaveTableButton = new Button((0, 0, 20, 20), "LeaveTable", "X", LeaveTable);
            AddActor(leaveTableButton);
            leaveTableButton.MoveTo(610, 10);
            gameInfoBar = new GameInfoBar((44, 408));
            AddActor(gameInfoBar);
            Login login = Login.GetInstance();
            byte[] bytes = new byte[100];
            for(int i = 0; i < 100; ++i)
                bytes[i] = 3;
            //bytes[61] = 1;
            //bytes[66] = 2;
            //bytes[71] = 1;
            //bytes[76] = 2;
            //bytes[81] = 1;
            //bytes[86] = 2;

            //if(login.CurrentTable.Board != null)
            DrawBoard(5, bytes); //DrawBoard(login.CurrentTable.BoardSize, login.CurrentTable.Board);
        }
        public override void Dispose()
        {
        }

        private void DrawBoard(int boardSize, byte[] board)
        {
            var id = 0;
            int size = 20;
            int width = 640, height = 408;
            Tile tile = new Tile((width / 2-40, height / 2 - 30), id, size) { Owner = board[id] };
            
            AddActor(tile);
            ++id;
            for (int i = 1; i <= boardSize; i++)
            {
                for (int k = 0; k < 6; k++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        double x = width / 2 + (size * 3 / 2) * (k % 3 == 0 ? j : i) * (k < 3 ? 1 : -1) + (((size * 3 / 2 * j) * (k % 3 == 2 ? k == 2 ? -1 : 1 : 0))) - 40;
                        double y = height / 2 + i * size * Math.Sqrt(3) / (k % 3 == 0 ? 1 : 2) * (k > 1 && k < 5 ? 1 : -1) + size * Math.Sqrt(3) * j / (k % 3 == 1 ? 1 : 2) * (k < 3 ? 1 : -1) - 30;

                        AddActor(new Tile(((int)x, (int)y), id, size) { Owner = board[id] });
                        
                        ++id;
                    }
                }
                //int k = 0;
                //for (int j = 0; j < i; j++)
                //{
                //    double x = width / 2 + (size * 3 / 2) * (k % 3 == 0 ? j : i) * (k < 3 ? 1 : -1) + (((size * 3 / 2 * j) * (k % 3 == 2 ? k == 2 ? -1 : 1 : 0)));
                //    double y = height / 2 + i * size * Math.Sqrt(3) / (k % 3 == 0 ? 1 : 2) * (k > 1 && k < 5 ? 1 : -1) + size * Math.Sqrt(3) * j / (k % 3 == 1 ? 1 : 2) * (k < 3 ? 1 : -1);
                //    AddActor(new Tile(new Rectangle((int)x, (int)y), id, size));
                //    id++;
                //}
                //k = 1;
                //for (int j = 0; j < i; j++)
                //{
                //    double x = width / 2 + (size * 3 / 2) * (k % 3 == 0 ? j : i) * (k < 3 ? 1 : -1) + (((size * 3 / 2 * j) * (k % 3 == 2 ? k == 2 ? -1 : 1 : 0)));
                //    AddActor(new Tile(new Rectangle((int)x, (int)(height / 2 - i * size / 2 * Math.Sqrt(3) + size * Math.Sqrt(3) * j)), id, size));
                //    id++;
                //}
                //k = 2;
                //for (int j = 0; j < i; j++)
                //{
                //    double x = width / 2 + (size * 3 / 2) * (k % 3 == 0 ? j : i) * (k < 3 ? 1 : -1) + (((size * 3 / 2 * j) * (k % 3 == 2 ? k == 2 ? -1 : 1 : 0)));
                //    AddActor(new Tile(new Rectangle((int)x, (int)(height / 2 + i * size / 2 * Math.Sqrt(3) + size * Math.Sqrt(3) * j / 2)), id, size));
                //    id++;
                //}
                //k = 3;
                //for (int j = 0; j < i; j++)
                //{
                //    double x = width / 2 + (size * 3 / 2) * (k % 3 == 0 ? j : i) * (k < 3 ? 1 : -1) + (((size * 3 / 2 * j) * (k % 3 == 2 ? k == 2 ? -1 : 1 : 0)));
                //    AddActor(new Tile(new Rectangle((int)x, (int)(height / 2 + i * size * Math.Sqrt(3) - size * Math.Sqrt(3) * j / 2)), id, size));
                //    id++;
                //}
                //k = 4;
                //for (int j = 0; j < i; j++)
                //{
                //    double x = width / 2 + (size * 3 / 2) * (k % 3 == 0 ? j : i) * (k < 3 ? 1 : -1) + (((size * 3 / 2 * j) * (k % 3 == 2 ? k == 2 ? -1 : 1 : 0)));
                //    AddActor(new Tile(new Rectangle((int)x, (int)(height / 2 + i * size / 2 * Math.Sqrt(3) - size * Math.Sqrt(3) * j)), id, size));
                //    id++;
                //}
                //k = 5;
                //for (int j = 0; j < i; j++)
                //{
                //    double x = width / 2 + (size * 3 / 2) * (k % 3 == 0 ? j : i) * (k < 3 ? 1 : -1) + (((size * 3 / 2 * j) * (k % 3 == 2 ? k == 2 ? -1 : 1 : 0)));
                //    AddActor(new Tile(new Rectangle((int)x, (int)(height / 2 - i * size / 2 * Math.Sqrt(3) - size * Math.Sqrt(3) * j / 2)), id, size));
                //    id++;
                //}
            }
        }

        private void LeaveTable()
        {
            Login login = Login.GetInstance();
            LeaveTableRequest request = new LeaveTableRequest();
            request.Auth = login.GetAuth();
            request.TableNumber = login.CurrentTable.Number;
            IncomingMessageManager.AddIncomingMessageListener<LeaveTableResponse>(LeaveTableCallback);
            ConnectionManager.GetInstance().Send(request);
        }

        private void LeaveTableCallback(LeaveTableResponse response)
        {
            IncomingMessageManager.RemoveIncomingMessageListener<LeaveTableResponse>(LeaveTableCallback);
            if (response.Status == ResponseStatus.Denied)
                MessageBoxService.GetInstance(this).Show("Session expired. Please iog in again.", MessageBoxButtons.OK, SessionExpiredMessage, true);
            else if (response.Status == ResponseStatus.Error)
                MessageBoxService.GetInstance(this).Show("Internal server error. Please try again later.", MessageBoxButtons.OK, null, true);
            else
            {
                Login.GetInstance().UpdateToken(response.Auth.Token);
                Login.GetInstance().UpdateTableInfo(null);
                LevelManager.GetInstance().ChangeLevel<Room>();
            }
        }

        private void SessionExpiredMessage(MessageBoxResult result)
        {
            Login.GetInstance().Logout();
            LevelManager.GetInstance().ChangeLevel<MainMenu>();
        }
    }
}
