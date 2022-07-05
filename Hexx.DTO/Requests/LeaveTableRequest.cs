using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Requests
{
    [XmlRoot(ElementName = "LeaveTableRequest")]
    public class LeaveTableRequest : Request
    {
        [XmlElement(ElementName = "Auth")]
        public Auth Auth { get; set; }

        [XmlElement(ElementName = "TableNumber")]
        public int TableNumber { get; set; }
    }
}
