using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Requests
{
    [XmlRoot(ElementName = "LogoutRequest")]
    public class LogoutRequest : Request
    {
        [XmlElement(ElementName = "Auth")]
        public Auth Auth { get; set; }
    }
}
