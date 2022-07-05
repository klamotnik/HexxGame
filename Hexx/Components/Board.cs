using Hexx.Engine.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Components
{
    public class Board : Panel
    {
        public int Size { get; }
        private List<Tile> tiles { get; }

        public Board(int size) : base((0, 0))
        {
            Size = size;
            tiles = new List<Tile>();
            //for(int i = 0; i <)
        }

        private void DrawBoard(int boardSize, byte[] board)
        {
            var id = 0;
            int size = 20;
            int width = 640, height = 408;
            AddChild(new Tile((width / 2, height / 2), id, size) { Owner = board[id] });
            ++id;
            for (int i = 1; i <= boardSize; i++)
            {
                for (int k = 0; k < 6; k++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        double x = width / 2 + (size * 3 / 2) * (k % 3 == 0 ? j : i) * (k < 3 ? 1 : -1) + (((size * 3 / 2 * j) * (k % 3 == 2 ? k == 2 ? -1 : 1 : 0)));
                        double y = height / 2 + i * size * Math.Sqrt(3) / (k % 3 == 0 ? 1 : 2) * (k > 1 && k < 5 ? 1 : -1) + size * Math.Sqrt(3) * j / (k % 3 == 1 ? 1 : 2) * (k < 3 ? 1 : -1);
                        AddChild(new Tile(((int)x, (int)y), id, size) { Owner = board[id] });
                        ++id;
                    }
                }
            }
        }
    }
}
