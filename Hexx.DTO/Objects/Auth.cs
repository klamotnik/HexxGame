using System.Xml.Serialization;

namespace Hexx.DTO.Objects
{
    [XmlRoot(ElementName = "Hexx")]
    public class Auth
    {
        [XmlElement("User")]
        public int UserID { get; set; }

        [XmlElement("Token")]
        public string Token { get; set; }
    }
}
