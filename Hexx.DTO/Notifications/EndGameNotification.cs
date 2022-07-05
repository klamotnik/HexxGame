using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Notifications
{
    [XmlRoot(ElementName = "EndGameNotification")]
    public class EndGameNotification : Notification
    {
        [XmlElement(ElementName = "Table")]
        public Table Table { get; set; }
    }
}
