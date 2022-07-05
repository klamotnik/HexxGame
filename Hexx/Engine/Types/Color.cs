using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine.Types
{
    public sealed class Color
    {
        public static readonly Color White = new Color(0xFFFFFFFFu);
        public static readonly Color Black = new Color(0x000000FFu);
        public static readonly Color Red = new Color(0xFF0000FFu);
        public static readonly Color Green = new Color(0x00FF00FFu);
        public static readonly Color Blue = new Color(0x0000FFFFu);

        public CSSDL.Color Struct { get; private set; }
        
        public Color(byte r, byte g, byte b, byte a = 255)
        {
            Struct = new CSSDL.Color()
            {
                r = r,
                g = g,
                b = b,
                a = a
            };
        }

        public Color(uint rgba) : this((byte)((rgba & 0xFF000000) >> 24), (byte)((rgba & 0xFF0000) >> 16), (byte)((rgba & 0xFF00) >> 8), (byte)(rgba & 0xFF))
        {
        }

        public static implicit operator CSSDL.Color(Color color)
        {
            return color.Struct;
        }
    }
}
