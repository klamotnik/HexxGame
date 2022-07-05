using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL.Events
{
    [StructLayout(LayoutKind.Sequential)]
    public struct JoyBallEvent : IEvent
    {
        public EventType Type { get; }
        public UInt32 Timestamp { get; }
        public Int32 Which { get; }
        public byte Ball { get; }
        private UInt16 padding1;
        private byte padding2;
        public Int16 Xrel { get; }
        public Int16 Yrel { get; }
    }
}
