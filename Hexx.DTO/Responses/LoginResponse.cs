using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Responses
{
    [XmlRoot(ElementName = "LoginResponse")]
    public class LoginResponse : Response
    {
        [XmlElement(ElementName = "Auth")]
        public Auth Auth { get; set; }

        [XmlElement(ElementName = "Username")]
        public string Username { get; set; }
    }
}
