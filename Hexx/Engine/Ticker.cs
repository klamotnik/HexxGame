using System;
using System.Diagnostics;
using Hexx.Engine.Types;
using Hexx.Types;

namespace Hexx.Engine
{
    class Ticker : Singleton<Ticker>
    {
        public delegate void TickDelegate(int deltaTime);
        public delegate void TickWithNoTimeDelegate();
        public int LastTickTime { get; private set; }
        private static TickDelegate tick;
        private static TickWithNoTimeDelegate tickWithNoTime;
        private Stopwatch stopWatch = new Stopwatch();

        public Ticker()
        {
            stopWatch.Start();
        }

        public static void AddTickListener(ITickable tickable)
        {
            tick += tickable.Tick;
        }

        public static void AddTickListener(TickWithNoTimeDelegate action)
        {
            tickWithNoTime += action;
        }

        public static void RemoveTickListener(ITickable tickable)
        {
            tick -= tickable.Tick;
        }

        public static void RemoveTickListener(TickWithNoTimeDelegate action)
        {
            tickWithNoTime -= action;
        }

        public void Tick()
        {
            tickWithNoTime.Invoke();
            LastTickTime = (int)stopWatch.ElapsedMilliseconds;
            tick?.Invoke(LastTickTime);
            stopWatch.Restart();
        }
    }
}
