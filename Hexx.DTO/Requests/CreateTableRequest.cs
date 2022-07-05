using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Requests
{
    [XmlRoot(ElementName = "CreateTableRequest")]
    public class CreateTableRequest : Request
    {
        [XmlElement(ElementName = "Auth")]
        public Auth Auth { get; set; }

        [XmlElement(ElementName = "Table")]
        public Table Table { get; set; }
    }
}
