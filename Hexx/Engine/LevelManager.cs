using Hexx.Components.Levels;
using Hexx.Engine.Types;
using Hexx.Types;
using System;

namespace Hexx.Engine
{
    public delegate void ChangingLevel();
    public class LevelManager : Singleton<LevelManager>
    {
        public event ChangingLevel OnBeforeChangeLevel;
        public event ChangingLevel OnAfterChangeLevel;
        private Level currentLevel;

        public void ChangeLevel<T>() where T : Level
        {
            OnBeforeChangeLevel?.Invoke();
            Level level = (T)Activator.CreateInstance(typeof(T), new object[] { new Viewport(new CSSDL.Rectangle() { x = 0, y = 0, w = 640, h = 480 }) });
            if (currentLevel != null)
            {
                Ticker.RemoveTickListener(currentLevel);
                currentLevel.Viewport.Dispose();
                currentLevel.Dispose();
            }
            currentLevel = level;
            Ticker.AddTickListener(currentLevel);
            OnAfterChangeLevel?.Invoke();
        }

        public Level GetCurrentLevel()
        {
            if (currentLevel == null)
                ChangeLevel<MainMenu>();
            return currentLevel;
        }
    }
}
