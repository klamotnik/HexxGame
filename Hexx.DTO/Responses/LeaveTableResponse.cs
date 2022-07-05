using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Responses
{
    [XmlRoot(ElementName = "LeaveTableResponse")]
    public class LeaveTableResponse : Response
    {
        [XmlElement(ElementName = "Auth")]
        public Auth Auth { get; set; }
    }
}
