using System.Xml.Serialization;

namespace Hexx.DTO
{
    public enum NotificationAction
    {
        New,
        Modify,
        Delete
    }
    
    public abstract class Notification : Message
    {
        [XmlElement(ElementName = "Action")]
        public NotificationAction Action { get; set; }
    }
}
