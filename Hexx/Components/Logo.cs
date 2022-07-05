using CSSDL;
using Hexx.Engine.Types;

namespace Hexx.Components
{
    public class Logo : Actor
    {
        private static Viewport[] sprites = new Viewport[]
        {
            new Viewport(Surface.LoadSprite("sprites/logo.png"))
        };

        ~Logo()
        {
            foreach (Viewport v in sprites)
                v.Dispose();
        }

        public Logo(Rectangle rectangle) : base(sprites[0])
        {
            MoveTo(rectangle.x, rectangle.y);
        }
    }
}
