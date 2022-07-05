using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL.Events
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SysWMEvent : IEvent
    {
        public EventType Type { get; }
        public UInt32 Timestamp { get; }
        public IntPtr Message { get; }
    }
}
