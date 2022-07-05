using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Requests
{
    [XmlRoot(ElementName = "EnterTableRequest")]
    public class EnterTableRequest : Request
    {
        [XmlElement(ElementName = "Auth")]
        public Auth Auth { get; set; }

        [XmlElement(ElementName = "TableNumber")]
        public int TableNumber { get; set; }
    }
}
