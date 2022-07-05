using Hexx.Engine.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hexx.Engine
{
    public class SEngine
    {
        private Window window;
        private bool end;

        public SEngine()
        {
            CSSDL.Core.Init(CSSDL.InitComponent.Everything);
            window = new Window(640, 480, "Hexx");
            EventManager.AddQuitListener(OnQuit);
        }
        
        public void Start()
        {
            LevelManager levelManager = LevelManager.GetInstance();
            Ticker ticker = Ticker.GetInstance();
            while (!end && !EventManager.IsQuitRequested)
            {
                Level level = levelManager.GetCurrentLevel();
                ticker.Tick();
                level.Refresh();
                if (level.NeedRefresh)
                    window.DrawOn(level);
                Thread.Sleep(20 /* - (ticker.LastTickTime > 40 ? 20 : ticker.LastTickTime < 20 ? 0 : ticker.LastTickTime - 20)*/);
            }
            CSSDL.Core.Quit();
            Connection.ConnectionManager.GetInstance().Disconnect();
        }

        private void OnQuit(CSSDL.Events.QuitEvent e)
        {
            end = true;
        }
    }
}
