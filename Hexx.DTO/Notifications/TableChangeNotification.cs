using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Notifications
{
    [XmlRoot(ElementName = "TableChangeNotification")]
    public class TableChangeNotification : Notification
    {
        [XmlElement(ElementName = "Table")]
        public Table Table { get; set; }
    }
}
