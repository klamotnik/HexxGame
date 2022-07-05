using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Requests
{
    [XmlRoot(ElementName = "LeaveSeatRequest")]
    public class LeaveSeatRequest : Request
    {
        [XmlElement(ElementName = "Auth")]
        public Auth Auth { get; set; }
    }
}
