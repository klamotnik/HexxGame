using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hexx.DTO.Requests
{
    [XmlRoot(ElementName = "LoginRequest")]
    public class LoginRequest : Request
    {
        [XmlElement(ElementName = "Login")]
        public string Login { get; set; }
        [XmlElement(ElementName = "Password")]
        public string Password { get; set; }
    }
}
