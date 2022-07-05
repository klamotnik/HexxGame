using CSSDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine.Types
{
    public class Label : FormControl
    {
        public uint MaxWidth { get; set; }
        public string Text
        {
            get => text;
            set
            {
                text = value;
                NeedRefresh = true;
            }
        }
        public int FontSize { get; private set; }
        public Color FontColor
        {
            get => fontColor;
            set
            {
                fontColor = value;
                NeedRefresh = true;
            }
        }
        private Color fontColor;
        private string text;

        public Label(int fontSize, string text) : this(new Rectangle(), fontSize, text)
        {

        }

        public Label(Rectangle rectangle, int fontSize, string text) : base(text)
        {
            Viewport = new Viewport(rectangle);
            Text = text;
            FontSize = fontSize;
            FontColor = Color.Black;
        }

        public override void Refresh()
        {
            base.Refresh();
            if (!IsVisible)
                return;
            int x = Viewport.Properties.x;
            int y = Viewport.Properties.y;
            Viewport?.Dispose();
            if(MaxWidth > 0)
                Viewport = TextRenderer.GetInstance().RenderText("xolonium.ttf", FontSize, FontColor, Text, MaxWidth);
            else
                Viewport = TextRenderer.GetInstance().RenderText("xolonium.ttf", FontSize, FontColor, Text);
            Viewport.MoveTo(x, y);
        }
    }
}
