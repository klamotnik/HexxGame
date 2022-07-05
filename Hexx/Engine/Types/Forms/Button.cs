using CSSDL;
using CSSDL.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine.Types
{
    public delegate void ButtonClickAction();

    public class Button : FormControl
    {
        public uint FontSize
        {
            get => fontSize;
            set
            {
                fontSize = (value > 100 ? 100 : value == 0 ? 1 : value);
                RefreshButtonText();
            }
        }

        public string Text
        {
            get => text;
            set
            {
                text = value;
                RefreshButtonText();
            }
        }
        private uint fontSize;
        private string text;
        private ButtonClickAction buttonClickAction;
        private Viewport buttonText;

        public Button(Rectangle rectangle, string name, string text, ButtonClickAction action) : base(name)
        {
            Viewport = new Viewport(rectangle);
            Text = text;
            buttonClickAction = action;
            FontSize = 16;
            EventManager.AddMouseButtonDownListener(MouseClick);
        }

        public override void Refresh()
        {
            base.Refresh();
            Viewport.Clear();
            if (!IsVisible)
                return;
            DrawingProcessor dp = new DrawingProcessor(Viewport.Surface);
            dp.DrawRoundedBorder(Color.Black.Struct);
            dp.FillPolygon(3, 1, Color.White.Struct);
            Viewport.Draw(buttonText);
        }

        private void RefreshButtonText()
        {
            buttonText?.Dispose();
            buttonText = TextRenderer.GetInstance().RenderText("xolonium.ttf", (int)fontSize, Color.Black, Text);
            buttonText.MoveTo((Viewport.Properties.w - buttonText.Properties.w) / 2, (Viewport.Properties.h - buttonText.Properties.h) / 2);
        }

        private void MouseClick(MouseButtonEvent e)
        {
            if (IsVisible && CanInteract && AbsolutePosition.IsPointInRectangle(e.X, e.Y))
                buttonClickAction?.Invoke();
        }
    }
}
