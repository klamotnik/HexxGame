using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL.Events
{
    public enum ControllerAxis : sbyte
    {
        Invalid = -1,
        LeftX = 0,
        LeftY = 1,
        RightX = 2,
        RightY = 3,
        TriggerLeft = 4,
        TriggerRight = 5,
        Max = 6
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ControllerAxisEvent : IEvent
    {
        public EventType Type { get; }
        public UInt32 Timestamp { get; }
        public Int32 Which { get; }
        public ControllerAxis Axis { get; }
        private UInt16 padding1;
        private byte padding2;
        public Int16 Value { get; }
    }
}
