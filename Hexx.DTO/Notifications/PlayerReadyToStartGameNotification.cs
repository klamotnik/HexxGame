using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Notifications
{
    [XmlRoot(ElementName = "PlayerReadyToStartGameNotification")]
    public class PlayerReadyToStartGameNotification : Notification
    {
        [XmlElement(ElementName = "Table")]
        public Table Table { get; set; }
        [XmlElement(ElementName = "Seat")]
        public int Seat { get; set; }
    }
}
