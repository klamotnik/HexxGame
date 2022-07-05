using CSSDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine.Types
{
    public enum MessageBoxButtons
    {
        OK,
        YesNo,
        Abort
    }

    public enum MessageBoxResult
    {
        OK,
        Yes,
        No,
        Abort
    }

    public delegate void MessageBoxCallback(MessageBoxResult result);

    public class MessageBox : Actor
    {

        public bool Shadowed
        {
            get => shadowed;
            set
            {
                if (shadowed != value)
                {
                    shadowed = value;
                    NeedRefresh = true;
                }
            }
        }
        private bool shadowed;
        private Panel box;
        private Button[] buttons;
        private Label message;
        private MessageBoxCallback callback;

        public MessageBox(string text, MessageBoxButtons buttons, MessageBoxCallback callback)
        {
            message = new Label(new CSSDL.Rectangle(10, 10), 12, text);
            message.MaxWidth = 300;
            message.Refresh();
            this.buttons = GenerateButtons(buttons);
            box = new Panel(new CSSDL.Rectangle((640 - message.Viewport.Properties.w - 20) / 2, (480 - message.Viewport.Properties.h - 56) / 2, message.Viewport.Properties.w + 20, message.Viewport.Properties.h + 56));
            box.AddChild(message);
            box.AddChild(this.buttons);
            Viewport = new Viewport(new CSSDL.Rectangle(0, 0, 640, 480));
            this.callback = callback;
        }

        public override void Refresh()
        {
            base.Refresh();
            if (IsVisible)
            {
                if (shadowed)
                    Viewport.Fill(new Color(0x0000007Fu));
                DrawingProcessor dp = new DrawingProcessor(box.Viewport.Surface);
                dp.DrawRoundedBorder(new Color(255, 255, 255));
                dp.FillPolygon(10, 10, new Color(255, 255, 255));
                Viewport.Draw(box);
                if (box.NeedRefresh)
                    box.Refresh();
                Viewport.Draw(box);
            }
        }

        private Button[] GenerateButtons(MessageBoxButtons buttons)
        {
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    return new[]
                    {
                        new Button(((message.Viewport.Properties.w + 20) / 2 - 40, message.Viewport.Properties.h + 20, 80, 26), "OK", "OK", () => ButtonClick(MessageBoxResult.OK))
                    };
                case MessageBoxButtons.Abort:
                    return new[]
                    {
                        new Button(((message.Viewport.Properties.w + 20) / 2 - 40, message.Viewport.Properties.h + 20, 80, 26), "Abort", "Abort", () => ButtonClick(MessageBoxResult.Abort))
                    };
                case MessageBoxButtons.YesNo:
                    return new[]
                    {
                        new Button(((message.Viewport.Properties.w + 20) / 2 - 85, message.Viewport.Properties.h + 20, 80, 26), "Yes", "Yes", () => ButtonClick(MessageBoxResult.Yes)),
                        new Button(((message.Viewport.Properties.w + 20) / 2 + 5, message.Viewport.Properties.h + 20, 80, 26), "No", "No", () => ButtonClick(MessageBoxResult.No))
                    };
            }
            return null;
        }

        public void ButtonClick(MessageBoxResult result)
        {
            foreach (Button button in buttons)
                button.CanInteract = false;
            IsVisible = false;
            callback?.Invoke(result);
        }
    }
}
