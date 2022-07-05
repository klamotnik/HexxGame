using CSSDL.Events;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Hexx.Engine
{
    public delegate void EventDelegate<T>(T e) where T : IEvent;

    public static class EventManager
    {
        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool SDL_PollEvent(IntPtr ev);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SDL_PeepEvents(IntPtr ev, int numevents, int action, UInt32 minType, UInt32 maxType);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SDL_PumpEvents();

        public static bool IsQuitRequested
        {
            get
            {
                SDL_PumpEvents();
                return SDL_PeepEvents(IntPtr.Zero, 0, 1, (UInt32)CSSDL.EventType.Quit, (UInt32)CSSDL.EventType.Quit) + SDL_PeepEvents(IntPtr.Zero, 0, 1, (UInt32)CSSDL.EventType.AppTerminating, (UInt32)CSSDL.EventType.AppTerminating) > 0;
            }
        }
        private static EventDelegate<KeyboardEvent> keyDownListeners;
        private static EventDelegate<KeyboardEvent> keyUpListeners;
        private static EventDelegate<MouseMotionEvent> mouseMotionListeners;
        private static EventDelegate<MouseButtonEvent> mouseButtonDownListeners;
        private static EventDelegate<MouseButtonEvent> mouseButtonUpListeners;
        private static EventDelegate<QuitEvent> quitListeners;

        private static CSSDL.Event ev = new CSSDL.Event();
        private static IntPtr evPtr;

        static EventManager()
        {
            Ticker.AddTickListener(BroadcastPendingEvents);
            evPtr = Marshal.AllocHGlobal(Marshal.SizeOf(ev));
            LevelManager.GetInstance().OnBeforeChangeLevel += LevelChanged;
        }

        public static void AddKeyDownListener(EventDelegate<KeyboardEvent> action)
        {
            keyDownListeners += action;
        }

        public static void RemoveKeyDownListener(EventDelegate<KeyboardEvent> action)
        {
            keyDownListeners -= action;
        }

        public static void AddKeyUpListener(EventDelegate<KeyboardEvent> action)
        {
            keyUpListeners += action;
        }

        public static void RemoveKeyUpListener(EventDelegate<KeyboardEvent> action)
        {
            keyUpListeners -= action;
        }

        public static void AddMouseMotionListener(EventDelegate<MouseMotionEvent> action)
        {
            mouseMotionListeners += action;
        }

        public static void RemoveMouseMotionListener(EventDelegate<MouseMotionEvent> action)
        {
            mouseMotionListeners -= action;
        }

        public static void AddMouseButtonDownListener(EventDelegate<MouseButtonEvent> action)
        {
            mouseButtonDownListeners += action;
        }

        public static void RemoveMouseButtonDownListener(EventDelegate<MouseButtonEvent> action)
        {
            mouseButtonDownListeners -= action;
        }

        public static void AddMouseButtonUpListener(EventDelegate<MouseButtonEvent> action)
        {
            mouseButtonUpListeners += action;
        }

        public static void RemoveMouseButtonUpListener(EventDelegate<MouseButtonEvent> action)
        {
            mouseButtonUpListeners -= action;
        }

        public static void AddQuitListener(EventDelegate<QuitEvent> action)
        {
            quitListeners += action;
        }

        public static void RemoveQuitListener(EventDelegate<QuitEvent> action) 
            => quitListeners -= action;

        static void BroadcastPendingEvents()
        {
            while (SDL_PollEvent(evPtr))
            {
                ev = (CSSDL.Event)Marshal.PtrToStructure(evPtr, typeof(CSSDL.Event));
                switch (ev.Type)
                {
                    case CSSDL.EventType.Quit:
                        quitListeners?.Invoke(ev.Quit);
                        break;
                    case CSSDL.EventType.KeyDown:
                        keyDownListeners?.Invoke(ev.Keyboard);
                        break;
                    case CSSDL.EventType.KeyUp:
                        keyUpListeners?.Invoke(ev.Keyboard);
                        break;
                    case CSSDL.EventType.MouseMotion:
                        mouseMotionListeners?.Invoke(ev.MouseMotion);
                        break;
                    case CSSDL.EventType.MouseButtonDown:
                        mouseButtonDownListeners?.Invoke(ev.MouseButton);
                        break;
                    case CSSDL.EventType.MouseButtonUp:
                        mouseButtonUpListeners?.Invoke(ev.MouseButton);
                        break;
                }
            }
        }

        static void LevelChanged()
        {
            keyDownListeners = null;
            keyUpListeners = null;
            mouseMotionListeners = null;
            mouseButtonDownListeners = null;
            mouseButtonUpListeners = null;
        }
    }
}
