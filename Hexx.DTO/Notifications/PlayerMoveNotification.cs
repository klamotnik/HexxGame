using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Notifications
{
    [XmlRoot(ElementName = "PlayerMoveNotification")]
    public class PlayerMoveNotification : Notification
    {
        [XmlElement(ElementName = "Table")]
        public Table Table { get; set; }
        [XmlElement(ElementName = "TurnForSeat")]
        public int TurnForSeat { get; set; }
    }
}
