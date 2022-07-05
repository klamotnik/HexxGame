using System;
using System.Runtime.InteropServices;

namespace CSSDL
{
    public class Window : BaseComponent
    {
        public enum WindowPosition { Undefined = 0x1FFF0000 }
        public enum WindowFlags { Fullscreen = 0x00000001, UseOpenGL = 0x00000002, Visible = 0x00000004, Hidden = 0x00000008, Borderless = 0x00000010, Resizable = 0x00000020, Minimized = 0x00000040, Maximized = 0x00000080, InputGrabbeed = 0x00000100, InputFocus = 0x00000200, MouseFocus = 0x00000400, FillscreenDesktop = 0x00001001, NotSdlWindow = 0x00000800, HighDpiMode = 0x00002000, MouseCapture = 0x00004000, AlwaysOnTop = 0x00008000, NotInTaskBar = 0x00010000, UtilityWindow = 0x00020000, Tooltip = 0x00040000, PopupMenu = 0x00080000 }

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_CreateWindow(string title, int x, int y, int w, int h, WindowFlags flags);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_GetWindowSurface(IntPtr window);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr SDL_UpdateWindowSurface(IntPtr window);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_DestroyWindow(IntPtr window);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_SetWindowIcon(IntPtr window, IntPtr icon);

        public Window(int width, int height, string title) : base(IntPtr.Zero)
        {
            if (width > 0 && height > 0)
            {
                this.title = title;
                handle = SDL_CreateWindow(title, (int)WindowPosition.Undefined, (int)WindowPosition.Undefined, width, height, WindowFlags.Visible);
                windowSurface = new Surface(SDL_GetWindowSurface(this), 0, 0);
            }
            else
                throw new ArgumentException("Width and height minimum value is 1px");
        }

        public void DrawOnWindow(Surface surface)
        {
            windowSurface.Fill(new Color() { r = 64, g = 0, b = 0, a = 255 });
            windowSurface.Draw(surface);
            SDL_UpdateWindowSurface(this);
        }

        public void SetWindowIcon(Surface icon)
        {
            SDL_SetWindowIcon(handle, icon.handle);
        }

        public override void CloseHandle()
        {
            windowSurface.Dispose();
            SDL_DestroyWindow(handle);
        }

        private Surface windowSurface;
        private string title;
    }
}
