using System.Xml.Serialization;

namespace Hexx.DTO.Objects
{
    [XmlRoot(ElementName = "RoomInfo")]
    public class RoomInfo
    {
        [XmlElement(ElementName = "Tables")]
        public Tables Tables { get; set; }
        [XmlElement(ElementName = "Players")]
        public Players Players { get; set; }
    }
}
