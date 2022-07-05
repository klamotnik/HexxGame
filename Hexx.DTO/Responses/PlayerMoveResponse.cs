using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Responses
{
    [XmlRoot(ElementName = "PlayerMoveResponse")]
    public class PlayerMoveResponse : Response
    {
        [XmlElement(ElementName = "Auth")]
        public Auth Auth { get; set; }
    }
}
