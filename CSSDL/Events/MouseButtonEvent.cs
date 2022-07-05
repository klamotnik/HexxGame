using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL.Events
{
    public enum MouseButton : byte
    {
        Left = 1,
        Middle = 2,
        Right = 3,
        X1 = 4,
        X2 = 5
    }

    public enum MouseButtonState : byte
    {
        Released = 0,
        Pressed = 1
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MouseButtonEvent : IEvent
    {
        public EventType Type { get; }
        public UInt32 Timestamp { get; }
        public UInt32 WindowID { get; }
        public UInt32 Which { get; }
        public MouseButton Button { get; }
        public MouseButtonState State { get; }
        public byte Clicks { get; }
        private byte padding1;
        public Int32 X { get; }
        public Int32 Y { get; }
    }
}
