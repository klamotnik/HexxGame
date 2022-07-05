using System.Runtime.InteropServices;

namespace CSSDL
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Color
    {
        public static readonly Color White = new Color() { r = 255, g = 255, b = 255, a = 255 };
        public static readonly Color Black = new Color() { r = 0, g = 0, b = 0, a = 255 };
        public static readonly Color Red = new Color() { r = 255, g = 0, b = 0, a = 255 };
        public static readonly Color Green = new Color() { r = 0, g = 255, b = 0, a = 255 };
        public static readonly Color Blue = new Color() { r = 0, g = 0, b = 255, a = 255 };
        public static readonly Color Yellow = new Color() { r = 255, g = 255, b = 0, a = 255 };

        public byte r { get; set; }
        public byte g { get; set; }
        public byte b { get; set; }
        public byte a { get; set; }

        public uint ToUint()
        {
            return ((uint)a << 24) + ((uint)b << 16) + ((uint)g << 8) + (uint)r;
        }

        public int ToInt()
        {
            return (int)(((uint)a << 24) + ((uint)b << 16) + ((uint)g << 8) + (int)r);
        }
    }
}
