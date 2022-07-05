using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine.Types
{
    public class MenuContainer : Actor
    {
        private OrderedDictionary menuElements = new OrderedDictionary(); //OrderedDictionary<string, MenuElement> menuElements = new Dictionary<string, MenuElement>();
        private string activeElement;

        public MenuContainer() : base()
        {
            needRefresh = true;
            EventManager.AddKeyDownListener(ChangeActiveElementByKeyboard);
        }

        private void ChangeActiveElementByKeyboard(CSSDL.Events.KeyboardEvent e)
        {
            MenuElement active = (MenuElement)menuElements[activeElement];
            int index;
            switch (e.Key.KeyCode)
            {
                case CSSDL.Events.Structures.KeyCode.Up:
                    active.Deactivate();
                    index = GetIndex(menuElements, activeElement);
                    if (index == 0)
                        active = (MenuElement)menuElements[menuElements.Count - 1];
                    else
                        active = (MenuElement)menuElements[index - 1];
                    active.Activate();
                    break;
                case CSSDL.Events.Structures.KeyCode.Down:
                    active.Deactivate();
                    index = GetIndex(menuElements, activeElement);
                    if (index == menuElements.Count - 1)
                        active = (MenuElement)menuElements[0];
                    else
                        active = (MenuElement)menuElements[index + 1];
                    active.Activate();
                    break;
                case CSSDL.Events.Structures.KeyCode.Return:
                    active.InvokeAction();
                    break;
            }
            activeElement = active
            needRefresh = true;
        }

        public int GetIndex(OrderedDictionary dictionary, object key)
        {
            if (dictionary.Contains(key)) {
                object value = dictionary[key];
                for (int i = 0; i < dictionary.Count; ++i)
                    if (dictionary[key] == value)
                        return i;
            }
            return -1;
        }
    }
}
