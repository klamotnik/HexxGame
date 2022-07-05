using CSSDL;
using CSSDL.Events;
using Hexx.Engine.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Components
{
    public enum TileState
    {
        Selected,
        NearNeightbor,
        FarNeightbor,
        Neutral
    }

    public class Tile : Actor
    {
        public int ID { get; }
        public int Owner { get; set; }
        public TileState State { get; set; }
        private readonly static CSSDL.Color selectedTileBorderColor = CSSDL.Color.Blue;
        private readonly static CSSDL.Color nearNeightborTileBorderColor = CSSDL.Color.Green;
        private readonly static CSSDL.Color farNeightborTileBorderColor = CSSDL.Color.Yellow;
        private readonly static CSSDL.Color neutralTileBorderColor = CSSDL.Color.Black;
        private readonly static CSSDL.Color Player1TileColor = CSSDL.Color.Red;
        private readonly static CSSDL.Color Player2TileColor = CSSDL.Color.Blue;
        private readonly static CSSDL.Color NeutralTileColor = CSSDL.Color.White;
        private int boardSize;
        private Rectangle[] hexxVertex;
        
        public Tile(Rectangle rectangle, int id, int size) : base(rectangle)
        {
            ID = id;
            boardSize = size;
            State = TileState.Neutral;
            Viewport = new Viewport((rectangle.x, rectangle.y, 80, 70));
        }
        public override void Refresh()
        {
            base.Refresh();
            if (!IsVisible)
                return;
            double size = this.boardSize;
            double x = Viewport.Properties.w / 2, y = Viewport.Properties.h / 2;
            CSSDL.Color currentTileBorderColor = CSSDL.Color.Black;
            CSSDL.Color currentTileColor = CSSDL.Color.White;
            currentTileBorderColor = GetTileBorderColor();
            currentTileColor = GetTileColor();
            hexxVertex = new Rectangle[]
            {
                ((int)(x - size / 2), (int)(y - size * Math.Sqrt(3) / 2)),
                ((int)(x + size / 2), (int)(y - size * Math.Sqrt(3) / 2)),
                ((int)(x + size), (int)y),
                ((int)(x + size / 2), (int)(y + size * Math.Sqrt(3) / 2)),
                ((int)(x - size / 2), (int)(y + size * Math.Sqrt(3) / 2)),
                ((int)(x - size), (int)y)
            };

            DrawingProcessor dp = new DrawingProcessor(Viewport.Surface);
            for (int i = 0; i < 6; ++i)
                dp.DrawLine(hexxVertex[i].x, hexxVertex[i].y, hexxVertex[(i + 1) % 6].x, hexxVertex[(i + 1) % 6].y, 1, currentTileBorderColor);
            dp.FillPolygon(Viewport.Properties.w / 2, Viewport.Properties.h / 2, currentTileColor);
        }

        private CSSDL.Color GetTileBorderColor()
        {
            switch (State)
            {
                case TileState.Neutral:
                    return neutralTileBorderColor;
                case TileState.Selected:
                    return selectedTileBorderColor;
                case TileState.NearNeightbor:
                    return nearNeightborTileBorderColor;
                case TileState.FarNeightbor:
                    return farNeightborTileBorderColor;
                default:
                    return neutralTileBorderColor;
            }
        }

        public bool IsPointInPoly(Rectangle point)
        {
            bool c = false;
            for (int i = -1, j = hexxVertex.Length - 4; ++i < hexxVertex.Length - 3; j = i)
            {
                var x = ((hexxVertex[i].y <= point.y && point.y < hexxVertex[j].y) || (hexxVertex[j].y <= point.y && point.y < hexxVertex[i].y))
                && (point.x < (hexxVertex[j].x - hexxVertex[i].x) * (point.y - hexxVertex[i].y) / (hexxVertex[j].y - hexxVertex[i].y) + hexxVertex[i].x)
                && (c = !c);
            }
            return c;
        }

        private CSSDL.Color GetTileColor()
        {
            switch (Owner)
            {
                case 1:
                    return Player1TileColor;
                case 2:
                    return Player2TileColor;
            }
            return NeutralTileColor;
        }
    }
}
