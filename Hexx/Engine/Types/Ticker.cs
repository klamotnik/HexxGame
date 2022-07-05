using Hexx.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine.Types
{
    public class Ticker
    {
        private delegate void TickDelegate(int deltaTime);
        private TickDelegate tick;
        private long lastInvoke;

        public Ticker()
        {
            lastInvoke = GetCurrentTimestamp();
        }

        public static Ticker operator+(Ticker ticker, ITickable tickable)
        {
            if (ticker.tick == null)
                ticker.tick = new TickDelegate(tickable.Tick);
            else
                ticker.tick += tickable.Tick;
            return ticker;
        }

        public static Ticker operator-(Ticker ticker, ITickable tickable)
        {
            if (ticker.tick != null)
                ticker.tick -= tickable.Tick;
            return ticker;
        }

        public void Broadcast()
        {
            if (tick == null)
                return;
            long currentTimestamp = GetCurrentTimestamp();
            tick.Invoke((int)(currentTimestamp - lastInvoke));
            lastInvoke = currentTimestamp;
        }

        private long GetCurrentTimestamp()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}
