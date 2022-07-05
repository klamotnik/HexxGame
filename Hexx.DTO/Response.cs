using System;
using System.Xml.Serialization;

namespace Hexx.DTO
{
    [Flags]
    public enum ResponseStatus { OK, Denied, Error }

    public abstract class Response : Message
    {
        [XmlElement("Status")]
        public ResponseStatus Status { get; set; }
    }
}
