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
    public class Engine
    {
        private Window window;
        private bool end;

        public Engine()
        {
            CSSDL.Core.Init(CSSDL.InitComponent.Everything);
            window = new Window(640, 480, "Hexx");

            EventManager.AddQuitListener(OnQuit);
        }

        public void Start()
        {
            LevelManager levelManager = LevelManager.GetInstance();
            Ticker ticker = Ticker.GetInstance();
            while (!end)
            {
                Level level = levelManager.GetCurrentLevel();
                ticker.Tick();
                level.Refresh();
                window.DrawOn(level);
                Thread.Sleep(20);
                CSSDL.Core.Quit();
            }
        }

        private void OnQuit(CSSDL.Events.QuitEvent e)
        {
            end = true;
        }
    }
}
