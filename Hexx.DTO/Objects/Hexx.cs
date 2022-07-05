using Hexx.DTO.Notifications;
using Hexx.DTO.Requests;
using Hexx.DTO.Responses;
using System.Xml.Serialization;

namespace Hexx.DTO.Objects
{
    [XmlType(IncludeInSchema = false)]
    public enum ActionType
    {
        RegisterRequest,
        RegisterResponse,
        LoginRequest,
        LoginResponse,
        LogoutRequest,
        LogoutResponse,
        GetRoomInfoRequest,
        GetRoomInfoResponse,
        CreateTableRequest,
        CreateTableResponse,
        EnterTableRequest,
        EnterTableResponse,
        LeaveTableRequest,
        LeaveTableResponse,
        TakeSeatRequest,
        TakeSeatResponse,
        LeaveSeatRequest,
        LeaveSeatResponse,
        StartGameRequest,
        StartGameResponse,
        PlayerMoveRequest,
        PlayerMoveResponse,
        PlayerChangeNotification,
        TableChangeNotification,
        PlayerReadyToStartGameNotification,
        PlayerMoveNotification,
        GameStartedNotification,
        EndGameNotification
    }

    [XmlRoot(ElementName = "Hexx")]
    public class Hexx
    {
        [XmlChoiceIdentifier("Type")]
        [XmlElement("RegisterRequest", typeof(RegisterRequest))]
        [XmlElement("RegisterResponse", typeof(RegisterResponse))]
        [XmlElement("LoginRequest", typeof(LoginRequest))]
        [XmlElement("LoginResponse", typeof(LoginResponse))]
        [XmlElement("LogoutRequest", typeof(LogoutRequest))]
        [XmlElement("LogoutResponse", typeof(LogoutResponse))]
        [XmlElement("GetRoomInfoRequest", typeof(GetRoomInfoRequest))]
        [XmlElement("GetRoomInfoResponse", typeof(GetRoomInfoResponse))]
        [XmlElement("CreateTableRequest", typeof(CreateTableRequest))]
        [XmlElement("CreateTableResponse", typeof(CreateTableResponse))]
        [XmlElement("EnterTableRequest", typeof(EnterTableRequest))]
        [XmlElement("EnterTableResponse", typeof(EnterTableResponse))]
        [XmlElement("LeaveTableRequest", typeof(LeaveTableRequest))]
        [XmlElement("LeaveTableResponse", typeof(LeaveTableResponse))]
        [XmlElement("TakeSeatRequest", typeof(TakeSeatRequest))]
        [XmlElement("TakeSeatResponse", typeof(TakeSeatResponse))]
        [XmlElement("LeaveSeatRequest", typeof(LeaveSeatRequest))]
        [XmlElement("LeaveSeatResponse", typeof(LeaveSeatResponse))]
        [XmlElement("StartGameRequest", typeof(StartGameRequest))]
        [XmlElement("StartGameResponse", typeof(StartGameResponse))]
        [XmlElement("PlayerMoveRequest", typeof(PlayerMoveRequest))]
        [XmlElement("PlayerMoveResponse", typeof(PlayerMoveResponse))]
        [XmlElement("PlayerChangeNotification", typeof(PlayerChangeNotification))]
        [XmlElement("TableChangeNotification", typeof(TableChangeNotification))]
        [XmlElement("PlayerReadyToStartGameNotification", typeof(PlayerReadyToStartGameNotification))]
        [XmlElement("PlayerMoveNotification", typeof(PlayerMoveNotification))]
        [XmlElement("GameStartedNotification", typeof(GameStartedNotification))]
        [XmlElement("EndGameNotification", typeof(EndGameNotification))]
        public object Action { get; set; }

        [XmlIgnore]
        public ActionType Type { get; set; }
    }
}
