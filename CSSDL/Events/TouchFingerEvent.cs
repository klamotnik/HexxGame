using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL.Events
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TouchFingerEvent : IEvent
    {
        public EventType Type { get; }
        public UInt32 Timestamp { get; }
        public Int64 TouchId { get; }
        public Int64 FingerId { get; }
        public float X { get; }
        public float Y { get; }
        public float DeltaX { get; }
        public float DeltaY { get; }
        public float Pressure { get; }
    }
}
