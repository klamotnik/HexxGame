using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Notifications
{
    [XmlRoot(ElementName = "PlayerChangeNotification")]
    public class PlayerChangeNotification : Notification
    {
        [XmlElement(ElementName = "Player")]
        public Player Player { get; set; }
    }
}
