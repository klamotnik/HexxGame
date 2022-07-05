using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL.Events
{
    public enum MouseWheelDirection
    {
        Normal = 0,
        Flipped = 1
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MouseWheelEvent : IEvent
    {
        public EventType Type { get; }
        public UInt32 Timestamp { get; }
        public UInt32 WindowID { get; }
        public UInt32 Which { get; }
        public Int32 X { get; }
        public Int32 Y { get; }
        public MouseWheelDirection Direction { get; }
    }
}
