using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Notifications
{
    [XmlRoot(ElementName = "GameStartedNotification")]
    public class GameStartedNotification : Notification
    {
        [XmlElement(ElementName = "Table")]
        public Table Table { get; set; }
        [XmlElement(ElementName = "Seat")]
        public int Seat { get; set; }
    }
}
