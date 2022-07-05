using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL.Events
{
    public enum ControllerButton : sbyte
    { 
        Invalid = -1,
        A = 0,
        B = 1,
        X = 2,
        Y = 3,
        Back = 4,
        Guide = 5,
        Start = 6,
        LeftStick = 7,
        RightStick = 8,
        LeftShoulder = 9,
        RightShoulder = 10,
        DPadUp = 11,
        DPadDown = 12,
        DPadLeft = 13,
        DPadRight = 14,
        Max = 15
    }

    public enum ControllerButtonState : byte
    {
        Released = 0,
        Pressed = 1
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ControllerButtonEvent : IEvent
    {
        public EventType Type { get; }
        public UInt32 Timestamp { get; }
        public Int32 Which { get; }
        public ControllerButton Button { get; }
        public ControllerButtonState State { get; }
    }
}
