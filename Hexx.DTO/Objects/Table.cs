using System.Xml.Serialization;

namespace Hexx.DTO.Objects
{
    [XmlRoot(ElementName = "Table")]
    public class Table
    {
        [XmlElement(ElementName = "Number")]
        public int Number { get; set; }
        [XmlElement(ElementName = "TimeForPlayer")]
        public int TimeForPlayer { get; set; }
        [XmlElement(ElementName = "BoardSize")]
        public int BoardSize { get; set; }
        [XmlElement(ElementName = "Seat1")]
        public string Seat1 { get; set; }
        [XmlElement(ElementName = "Seat2")]
        public string Seat2 { get; set; }
        [XmlElement(ElementName = "Board")]
        public byte[] Board { get; set; }
    }
}
