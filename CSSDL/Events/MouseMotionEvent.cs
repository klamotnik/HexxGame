using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL.Events
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseMotionEvent : IEvent
    {
        public EventType Type { get; }
        public UInt32 Timestamp { get; }
        public UInt32 WindowID { get; }
        public UInt32 Which { get; }
        public UInt32 State { get; }
        public Int32 X { get; }
        public Int32 Y { get; }
        public Int32 Xrel { get; }
        public Int32 Yrel { get; }
    }
}
