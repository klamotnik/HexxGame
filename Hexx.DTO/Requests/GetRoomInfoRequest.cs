using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Requests
{
    [XmlRoot(ElementName = "GetRoomInfoRequest")]
    public class GetRoomInfoRequest : Request
    {
        [XmlElement(ElementName = "Auth")]
        public Auth Auth { get; set; }
    }
}
