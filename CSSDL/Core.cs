using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL
{
    public enum InitComponent
    {
        Timer = 0x00000001,
        Audio = 0x00000010,
        Video = 0x00000020,
        Joystick = 0x00000200,
        Haptic = 0x00001000,
        GameController = 0x00002000,
        Events = 0x00004000,
        Everything = 0x00007231
    }

    public static class Core
    {
        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_Init(UInt32 flags);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_Quit();
        
        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_PushEvent(IntPtr e);

        public static bool Init(InitComponent components)
        {
            return SDL_Init((UInt32)components) < 0;
        }

        public static void Quit()
        {
            SDL_Quit();

            Event e = new Event()
            {
                Type = EventType.User
            };
            IntPtr ePtr = Marshal.AllocHGlobal(Marshal.SizeOf(e));
            Marshal.StructureToPtr(e, ePtr, false);
            int i = SDL_PushEvent(ePtr);/**/
        }
    }
}
