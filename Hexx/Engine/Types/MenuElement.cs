using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine.Types
{
    public class MenuElement : Actor
    {
        public delegate void MenuAction();
        public string Name { get; private set; }
        public string Text { get; private set; }
        public bool Active { get; private set; }
        private MenuAction menuAction;

        public MenuElement(string name, string text, MenuAction action)
        {

        }

        public void Activate()
        {
            if (Active)
                return;
            Active = true;
            needRefresh = true;
        }

        public void Deactivate()
        {
            if (!Active)
                return;
            Active = false;
            needRefresh = true;
        }

        public override void Refresh()
        {
            Viewport.Dispose();
            Viewport.
            needRefresh = false;
        }

        public void InvokeAction()
        {
            menuAction?.Invoke();
        }
    }
}
