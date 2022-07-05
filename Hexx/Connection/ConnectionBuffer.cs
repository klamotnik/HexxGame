using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Connection
{
    public class ConnectionBuffer
    {
        public enum BufferOperation { Get, Set }
        private static ConnectionBuffer instance;
        private List<string> buffer;

        private ConnectionBuffer()
        {
            buffer = new List<string>();
        }

        public static ConnectionBuffer GetInstance()
        {
            if (instance == null)
                instance = new ConnectionBuffer();
            return instance;
        }

        public object PerformOperation(BufferOperation operation, object data = null)
        {
            lock (buffer)
            {
                switch (operation)
                {
                    case BufferOperation.Get:
                        object returnValue = buffer;
                        buffer = new List<string>();
                        return returnValue;
                    case BufferOperation.Set:
                        if (data is IEnumerable<string> dataCollection)
                            buffer.AddRange(dataCollection);
                        else
                            buffer.Add((string)data);
                        return null;
                    default:
                        return null;
                }
            }
        }
    }
}
