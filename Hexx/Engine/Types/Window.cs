using CSSDL;
using System;

namespace Hexx.Engine.Types
{
    public class Window : IDisposable
    {
        public string Title { get; private set; }
        private CSSDL.Window window;

        public Window(int width, int height, string title)
        {
            window = new CSSDL.Window(width, height, title);
            SetWindowIcon();
        }

        public void DrawOn(IRenderable objectToDraw)
        {
            if (objectToDraw.NeedRefresh)
                window.DrawOnWindow(objectToDraw.Viewport.Surface);
        }

        public void DrawOn(Viewport viewport)
        {
            window.DrawOnWindow(viewport.Surface);
        }

        public void Dispose()
        {
            window.Dispose();
            GC.SuppressFinalize(this);
        }

        private void SetWindowIcon()
        {
            System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetCallingAssembly().Location);
            if (icon != null)
            {
                System.Drawing.Bitmap bitmap = icon.ToBitmap();
                Surface iconSurface = new Surface((0, 0, bitmap.Width, bitmap.Height));
                DrawingProcessor dp = new DrawingProcessor(iconSurface);
                for (int i = 0; i < bitmap.Width; i++)
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        System.Drawing.Color color = bitmap.GetPixel(i, j);
                        dp.SetPixel(i, j, new Color(color.R, color.G, color.B, color.A ));
                    }
                window.SetWindowIcon(iconSurface);
                iconSurface.Dispose();
            }
        }
    }
}
