using Hexx.DTO.Objects;
using Hexx.Engine.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Components.Forms
{
    public class PlayersListRow : ListRow
    {
        private Player playerInfo;
        private Label playerName;

        public PlayersListRow(Player player) : base((0, 0, 150, 16), player.Name)
        {
            playerInfo = player;
            playerName = new Label(12, player.Name);
            AddChild(playerName);
        }

        public override void Refresh()
        {
            base.Refresh();
            if (!IsVisible)
                return;
            if (childrens.Any(p => p.NeedRefresh))
                Viewport.Clear();
            Viewport.Fill(Color.White);
            CSSDL.DrawingProcessor dp = new CSSDL.DrawingProcessor(Viewport.Surface);
            dp.DrawLine(0, Viewport.Properties.h - 1, Viewport.Properties.w - 1, Viewport.Properties.h - 1, 1, Color.Black);
            foreach (Actor child in childrens)
            {
                if (child.NeedRefresh)
                    child.Refresh();
                Viewport.Draw(child);
            }
        }
    }
}
