using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSDL.Events
{
    public interface IEvent
    {
        EventType Type { get; }
        UInt32 Timestamp { get; }
    }
}
