using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Requests
{
    [XmlRoot(ElementName = "StartGameRequest")]
    public class StartGameRequest : Request
    {
        [XmlElement(ElementName = "Auth")]
        public Auth Auth { get; set; }
    }
}
