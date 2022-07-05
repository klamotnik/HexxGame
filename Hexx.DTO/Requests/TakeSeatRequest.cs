using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Requests
{
    [XmlRoot(ElementName = "TakeSeatRequest")]
    public class TakeSeatRequest : Request
    {
        [XmlElement(ElementName = "Auth")]
        public Auth Auth { get; set; }

        [XmlElement(ElementName = "TableNumber")]
        public int TableNumber { get; set; }

        [XmlElement(ElementName = "Seat")]
        public int Seat { get; set; }
    }
}
