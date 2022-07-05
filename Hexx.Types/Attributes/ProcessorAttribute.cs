using System;
using System.Collections.Generic;
using System.Text;

namespace Hexx.Types.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ProcessorAttribute : Attribute
    {
        public Type DataType { get; }

        public ProcessorAttribute(Type dataType)
        {
            DataType = dataType;
        }
    }
}
