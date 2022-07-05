using CSSDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine.Types
{
    public class Viewport : IDisposable
    {
        public Rectangle Properties { get; private set; }
        public Surface Surface { get; private set; }

        public Viewport(Rectangle rectangle)
        {
            Properties = rectangle;
            Surface = new Surface(rectangle);
        }

        public Viewport(Surface surface, int x = 0, int y = 0)
        {
            Surface = surface;
            Properties = (x, y, surface.Properties.w, surface.Properties.h);
        }

        public void Clear()
        {
            Surface.Clear();
        }
        
        public void Draw(IRenderable objectToDraw)
        {
            Surface.Draw(objectToDraw.Viewport.Surface);
        }

        public void Draw(Viewport objectToDraw, Rectangle? from = null, Rectangle? to = null)
        {
            Surface.Draw(objectToDraw.Surface, from, to);
        }

        public void Fill(Color color)
        {
            Surface.Fill(color.Struct);
        }

        public void MoveTo(int newX, int newY)
        {
            if (newX < 0 || newY < 0)
                throw new Exception("New coordinates cannot be negative.");
            Properties = (newX, newY, Properties.w, Properties.h);
            Surface.MoveTo(newX, newY);
        }

        public void Dispose()
        {
            Surface.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
