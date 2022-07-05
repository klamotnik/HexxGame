using System.Xml.Serialization;

namespace Hexx.DTO.Objects
{
    [XmlRoot(ElementName = "Tables")]
    public class Tables
    {
        [XmlElement(ElementName = "Table")]
        public Table[] Table { get; set; }
    }
}
