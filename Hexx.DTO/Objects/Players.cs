using System.Collections.Generic;
using System.Xml.Serialization;

namespace Hexx.DTO.Objects
{
    [XmlRoot(ElementName = "Players")]
    public class Players
    {
        [XmlElement(ElementName = "Player")]
        public Player[] Player { get; set; }
    }
}
