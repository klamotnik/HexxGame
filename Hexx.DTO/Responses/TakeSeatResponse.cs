using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Responses
{
    [XmlRoot(ElementName = "TakeSeatResponse")]
    public class TakeSeatResponse : Response
    {
        [XmlElement(ElementName = "Auth")]
        public Auth Auth { get; set; }
    }
}
