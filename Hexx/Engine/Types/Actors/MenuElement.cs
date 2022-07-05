using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine.Types
{
    public delegate void MenuAction();

    public class MenuElement : Actor, IInvokable
    {
        public string Name { get; private set; }
        public string Text { get; private set; }
        public bool Active { get; private set; }
        private MenuAction menuAction;

        public MenuElement(string name, string text, MenuAction action)
        {
            Name = name;
            Text = text;
            menuAction = action;
            CSSDL.Font font = FontLibrary.GetInstance().GetFont("xolonium.ttf", 20);
            Viewport = new Viewport(new CSSDL.Rectangle(0, 0, TextRenderer.GetInstance().GetRenderedTextWidth(Text, font), 24));
        }

        public void Activate()
        {
            if (Active)
                return;
            Active = true;
            NeedRefresh = true;
        }

        public void Deactivate()
        {
            if (!Active)
                return;
            Active = false;
            NeedRefresh = true;
        }

        public override void Refresh()
        {
            base.Refresh();
            if (!IsVisible)
                return;
            Viewport?.Surface.CloseHandle();
            FontLibrary fontLibrary = FontLibrary.GetInstance();
            CSSDL.Font font = fontLibrary.GetFont("xolonium.ttf", 20);
            Color color = Active ? Color.Red : Color.Green;
            Viewport = TextRenderer.GetInstance().RenderText(font, color, Text);
        }

        public void InvokeAction()
        {
            menuAction?.Invoke();
        }
    }
}
