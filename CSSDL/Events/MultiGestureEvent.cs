using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL.Events
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MultiGestureEvent : IEvent
    {
        public EventType Type { get; }
        public UInt32 Timestamp { get; }
        public Int64 TouchId { get; }
        public float DeltaTheta { get; }
        public float DeltaDist { get; }
        public float X { get; }
        public float Y { get; }
        public UInt16 NumFingers { get; }
        public UInt16 Padding { get; }
    }
}
