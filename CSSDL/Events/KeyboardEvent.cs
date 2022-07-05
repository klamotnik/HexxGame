using CSSDL.Events.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL.Events
{
    public enum KeyboardKeyState : byte
    {
        Released = 0,
        Pressed = 1
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardEvent : IEvent
    {
        public EventType Type { get; }
        public UInt32 Timestamp { get; }
        public UInt32 WindowID { get; }
        public KeyboardKeyState State { get; }
        public byte Repeat { get; }
        private UInt16 padding1;
        public Key Key { get; }
    }
}
