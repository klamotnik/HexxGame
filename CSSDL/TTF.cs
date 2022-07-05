using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL
{
    public static class TTF
    {
        [DllImport("SDL2_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int TTF_Init();

        [DllImport("SDL2_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int TTF_WasInit();

        [DllImport("SDL2_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int TTF_Quit();

        [DllImport("SDL2_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_RenderText_Solid(IntPtr font, string text, Color color);

        [DllImport("SDL2_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_RenderUTF8_Solid(IntPtr font, string text, Color color);

        [DllImport("SDL2_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_RenderUNICODE_Solid(IntPtr font, string text, Color color);

        [DllImport("SDL2_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_RenderText_Shaded(IntPtr font, string text, Color color, Color backgroundColor);

        [DllImport("SDL2_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_RenderUTF8_Shaded(IntPtr font, string text, Color color, Color backgroundColor);

        [DllImport("SDL2_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_RenderUNICODE_Shaded(IntPtr font, string text, Color color, Color backgroundColor);

        [DllImport("SDL2_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_RenderText_Blended(IntPtr font, string text, Color color);

        [DllImport("SDL2_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_RenderUTF8_Blended(IntPtr font, string text, Color color);

        [DllImport("SDL2_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_RenderUNICODE_Blended(IntPtr font, string text, Color color);

        [DllImport("SDL2_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_RenderText_Blended_Wrapped(IntPtr font, string text, Color color, uint maxWidth);

        [DllImport("SDL2_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_RenderUTF8_Blended_Wrapped(IntPtr font, string text, Color color, uint maxWidth);

        [DllImport("SDL2_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_RenderUNICODE_Blended_Wrapped(IntPtr font, string text, Color color, uint maxWidth);

        [DllImport("SDL2_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int TTF_SizeText(IntPtr font, string text, IntPtr w, IntPtr h);

        public static bool InitTTF()
        {
            if (TTF_WasInit() <= 0)
                return TTF_Init() == 0;
            return true;
        }
        
        public static void QuitTTF()
        {
            if (TTF_WasInit() > 0)
                TTF_Quit();
        }

        public static Surface RenderText(string text, Font font, Color color)
        {
            if (text == null)
                text = string.Empty;
            byte[] bytes = Encoding.ASCII.GetBytes(text);
            if (string.IsNullOrEmpty(text))
                return new Surface(new Rectangle());
            return new Surface(TTF_RenderText_Blended(font.handle, text, color), 0, 0);
        }

        public static Surface RenderText(string text, Font font, Color color, uint maxWidth)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(text);
            if (string.IsNullOrEmpty(text))
                return new Surface(new Rectangle());
            return new Surface(TTF_RenderText_Blended_Wrapped(font.handle, text, color, maxWidth), 0, 0);
        }

        public static int GetTextWidth(string text, Font font)
        {
            IntPtr w = Marshal.AllocHGlobal(4);
            TTF_SizeText(font.handle, text, w, IntPtr.Zero);
            return Marshal.ReadInt32(w);
        }
    }
}
