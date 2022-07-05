using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Responses
{
    [XmlRoot(ElementName = "StartGameResponse")]
    public class StartGameResponse : Response
    {
        [XmlElement(ElementName = "Auth")]
        public Auth Auth { get; set; }
    }
}
