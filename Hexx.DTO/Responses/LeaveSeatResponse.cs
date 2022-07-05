using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Responses
{
    [XmlRoot(ElementName = "LeaveSeatResponse")]
    public class LeaveSeatResponse : Response
    {
        [XmlElement(ElementName = "Auth")]
        public Auth Auth { get; set; }
    }
}
