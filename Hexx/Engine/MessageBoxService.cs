using Hexx.Engine.Types;
using Hexx.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine
{
    public delegate void PushMessageBoxDelegate(MessageBox messageBox);

    public class MessageBoxService : Multiton<Level, MessageBoxService>
    {
        private static Dictionary<Level, PushMessageBoxDelegate> pushMethods = new Dictionary<Level, PushMessageBoxDelegate>();
        private Level level;

        public new static MessageBoxService GetInstance(Level level)
        {
            MessageBoxService a = Multiton<Level, MessageBoxService>.GetInstance(level);
            a.level = level;
            return a;
        }

        public static bool Register(Level level, PushMessageBoxDelegate pushMessageBoxDelegate)
        {
            if (pushMethods.ContainsKey(level))
                return false;
            pushMethods.Add(level, pushMessageBoxDelegate);
            return true;
        }

        public static bool Unregister(Level level)
        {
            if (!pushMethods.ContainsKey(level))
                return false;
            pushMethods[level] = null;
            pushMethods.Remove(level);
            return true;
        }

        public void Show(string text, MessageBoxButtons buttons, MessageBoxCallback callback, bool shadowed)
        {
            if (pushMethods.ContainsKey(level))
            {
                MessageBox mbox = new MessageBox(text, buttons, callback);
                mbox.Shadowed = shadowed;
                pushMethods[level].Invoke(mbox);
            }
        }
    }
}
