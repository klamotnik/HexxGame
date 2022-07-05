using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL.Events
{
    public enum JoyButton : byte
    {
        Released = 0,
        Pressed = 1
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct JoyButtonEvent : IEvent
    {
        public EventType Type { get; }
        public UInt32 Timestamp { get; }
        public Int32 Which { get; }
        public byte Button { get; }
        public JoyButton State { get; }
    }
}
