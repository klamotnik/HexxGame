using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL
{
    public class Font : BaseComponent
    {
        [DllImport("SDL2_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_OpenFont(string path, int size);

        [DllImport("SDL2_ttf.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void TTF_CloseFont(IntPtr font);

        public int Size { get; private set; }
        public string Name { get; private set; }

        public Font(string path, int size) : base(IntPtr.Zero)
        {
            TTF.InitTTF();
            handle = TTF_OpenFont(path, size);
            Size = size;
            Name = new FileInfo(path).Name;
        }

        public override void CloseHandle()
        {
            TTF_CloseFont(handle);
        }
    }
}
