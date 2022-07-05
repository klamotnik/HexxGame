using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Requests
{
    [XmlRoot(ElementName = "PlayerMoveRequest")]
    public class PlayerMoveRequest : Request
    {
        [XmlElement(ElementName = "Auth")]
        public Auth Auth { get; set; }
        [XmlElement(ElementName = "FromTileID")]
        public int FromTileID { get; set; }
        [XmlElement(ElementName = "ToTileID")]
        public int ToTileID { get; set; }
    }
}
