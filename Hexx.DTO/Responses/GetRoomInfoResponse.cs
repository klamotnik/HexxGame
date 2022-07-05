using Hexx.DTO.Objects;
using System.Xml.Serialization;

namespace Hexx.DTO.Responses
{
    [XmlRoot(ElementName = "GetRoomInfoResponse")]
    public class GetRoomInfoResponse : Response
    {
        [XmlElement(ElementName = "Auth")]
        public Auth Auth { get; set; }
        [XmlElement(ElementName = "RoomInfo")]
        public RoomInfo RoomInfo { get; set; }
    }
}
