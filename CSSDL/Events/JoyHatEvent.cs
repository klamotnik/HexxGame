using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL.Events
{
    public enum JoyHatState : byte
    {
        Centered = 0,
        Up = 1,
        Right = 2, 
        Down = 4,
        Left = 8,
        RightUp = 3,
        RightDown = 6,
        LeftUp = 9,
        LeftDown = 12
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JoyHatEvent : IEvent
    {
        public EventType Type { get; }
        public UInt32 Timestamp { get; }
        public Int32 Which { get; }
        public byte Hat { get; }
        public JoyHatState Value { get; }
    }
}
