using CSSDL.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine.Types
{
    public class MenuContainer : Actor, IInteractable
    {
        protected new List<MenuElement> childrens = new List<MenuElement>();
        private MenuElement activeElement;
        public bool CanInteract { get; set; }

        public MenuContainer() : base()
        {
            Viewport = new Viewport(new CSSDL.Rectangle(0, 0));
            NeedRefresh = true;
            CanInteract = true;
            EventManager.AddKeyDownListener(ChangeActiveElementOrActivate);
            EventManager.AddMouseMotionListener(ChangeActiveElementByMouse);
            EventManager.AddMouseButtonDownListener(InvokeElementBehindMouse);
        }

        ~MenuContainer()
        {
            EventManager.RemoveKeyDownListener(ChangeActiveElementOrActivate);
            EventManager.RemoveMouseMotionListener(ChangeActiveElementByMouse);
            EventManager.RemoveMouseButtonDownListener(InvokeElementBehindMouse);
        }

        public bool AddElement(string name, string text, MenuAction action)
        {
            if (childrens.Any(p => p.Name == name))
                return false;
            MenuElement element = new MenuElement(name, text, action);
            if (childrens.Count == 0)
            {
                activeElement = element;
                element.Activate();
            }
            childrens.Add(element);

            int width = childrens.Max(p => p.Viewport.Properties.x + p.Viewport.Properties.w);
            int height = childrens.Sum(p => p.Viewport.Properties.h);

            Viewport = new Viewport(new CSSDL.Rectangle((640 - width) / 2, 250, width, height));
            NeedRefresh = true;
            return true;
        }

        public bool RemoveElement(MenuElement element)
        {
            if (!childrens.Contains(element))
                return false;
            childrens.Remove(element);
            return true;
        }

        public bool RemoveElement(string elementName)
        {
            MenuElement element = childrens.Where(p => p.Name == elementName).FirstOrDefault();
            if (element == null)
                return false;
            childrens.Remove(element);
            return true;
        }

        public void ClearMenu()
        {
            childrens.Clear();
            NeedRefresh = true;
        }

        public override void Refresh()
        {
            base.Refresh();
            if (!IsVisible)
                return;
            Viewport.Clear();
            int y = 0;
            foreach(MenuElement element in childrens)
            {
                if (element.NeedRefresh)
                    element.Refresh();
                element.MoveTo((Viewport.Properties.w - element.Viewport.Properties.w) / 2, y);
                y += element.Viewport.Properties.h;
                Viewport.Draw(element);
            }
        }

        private void ChangeActiveElementOrActivate(CSSDL.Events.KeyboardEvent e)
        {
            if (!IsVisible || !CanInteract || childrens.Count == 0)
                return;
            int currentIndex = childrens.IndexOf(activeElement);
            switch (e.Key.KeyCode)
            {
                case CSSDL.Events.Structures.KeyCode.Up:
                    childrens[currentIndex].Deactivate();
                    if (--currentIndex < 0)
                        currentIndex = childrens.Count - 1;
                    UpdateMenu(currentIndex);
                    break;
                case CSSDL.Events.Structures.KeyCode.Down:
                    childrens[currentIndex].Deactivate();
                    if (++currentIndex > childrens.Count - 1)
                        currentIndex = 0;
                    UpdateMenu(currentIndex);
                    break;
                case CSSDL.Events.Structures.KeyCode.Return:
                    activeElement?.InvokeAction();
                    break;
            }
        }

        private void UpdateMenu(int activeElementIndex)
        {
            childrens[activeElementIndex].Activate();
            activeElement = childrens[activeElementIndex];
            NeedRefresh = true;
        }

        private void InvokeElementBehindMouse(MouseButtonEvent e)
        {
            if (!IsVisible || !CanInteract || e.Button != MouseButton.Left)
                return;
            MenuElement element = GetElementByPoint(e.X, e.Y);
            element?.InvokeAction();
        }

        private void ChangeActiveElementByMouse(MouseMotionEvent e)
        {
            if (!IsVisible || !CanInteract)
                return;
            MenuElement element = GetElementByPoint(e.X, e.Y);
            if (element == null || element == activeElement)
                return;
            activeElement.Deactivate();
            activeElement = element;
            activeElement.Activate();
            NeedRefresh = true;
        }

        private MenuElement GetElementByPoint(int x, int y)
        {
            if (!Viewport.Properties.IsPointInRectangle(x, y))
                return null;
            int relX = x - Viewport.Properties.x;
            int relY = y - Viewport.Properties.y;
            MenuElement element = childrens.Where(p => p.Viewport.Properties.IsPointInRectangle(relX, relY)).FirstOrDefault();
            return element;
        }
    }
}
