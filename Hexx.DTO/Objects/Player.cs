using System.Xml.Serialization;

namespace Hexx.DTO.Objects
{
    [XmlRoot(ElementName = "Player")]
    public class Player
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Rank")]
        public int Rank { get; set; }
        [XmlElement(ElementName = "Table")]
        public int Table { get; set; }
        [XmlElement(ElementName = "Seat")]
        public int Seat { get; set; }
    }
}
