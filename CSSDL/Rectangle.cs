using System.Runtime.InteropServices;

namespace CSSDL
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle
    {
        public int x;
        public int y;
        public int w;
        public int h;

        public Rectangle(int x, int y) : this()
        {
            this.x = x;
            this.y = y;
            w = h = 0;
        }

        public Rectangle(int x, int y, int w, int h) : this()
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public bool IsPointInRectangle(int x, int y)
        {
            return this.x <= x && this.x + w >= x && this.y <= y && this.y + h >= y;
        }

        public static implicit operator Rectangle((int x, int y) coords)
        {
            return new Rectangle(coords.x, coords.y);
        }

        public static implicit operator Rectangle((int x, int y, int w, int h) rectangle)
        {
            return new Rectangle(rectangle.x, rectangle.y, rectangle.w, rectangle.h);
        }
    }
}
