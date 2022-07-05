using System;
using System.Linq;
using Hexx.DTO;
using Hexx.DTO.Objects;
using Hexx.DTO.Requests;
using Hexx.DTO.Responses;
using Hexx.Server.Db;
using Hexx.Server.Db.Entity;
using Hexx.Server.Types;
using Microsoft.EntityFrameworkCore;

namespace Hexx.Server.Processors
{
    class GetRoomInfoRequestProcessor : RequestProcessor<GetRoomInfoRequest>
    {
        private DatabaseContext context;
        private UserToken userToken;
        private bool accessGranted;

        public GetRoomInfoRequestProcessor(DTO.Objects.Hexx request) : base(request)
        {
            DatabaseConnectionManager connectionManager = DatabaseConnectionManager.GetInstance();
            context = new DatabaseContext();
        }

        public override Response Process()
        {
            Response response;
            try
            {
                UserTokenManager tokenManager = new UserTokenManager(context);
                accessGranted = tokenManager.Verify(request.Auth);
                if (accessGranted)
                    tokenManager.UpdateToken();
                userToken = tokenManager.userToken;
                response = GenerateResponse(GetResponseStatus());
                context.SaveChanges();
            }
            catch
            {
                response = GenerateResponse(ResponseStatus.Error);
            }
            return response;
        }
        
        protected override Response GenerateResponse(ResponseStatus status)
        {
            GetRoomInfoResponse response = new GetRoomInfoResponse();
            response.Status = status;
            if (status == ResponseStatus.OK || status == ResponseStatus.Error)
            {
                response.Auth = new Auth();
                response.Auth.UserID = userToken.ID;
                response.Auth.Token = userToken.Token;
                if(status == ResponseStatus.OK)
                    response.RoomInfo = GenerateRoomInfo();
            }
            return response;
        }

        private RoomInfo GenerateRoomInfo()
        {
            RoomInfo roomInfo = new RoomInfo()
            {
                Players = new Players(),
                Tables = new Tables()
            };
            IQueryable<Db.Entity.Table> tables = context.Tables.Include(p=>p.Player1)
                                                               .Include(p=>p.Player2);
            IQueryable<User> loggenInUsers = context.UserTokens.Include(p=>p.User)
                                                               .Select(p=>p.User);
            foreach (Db.Entity.Table table in tables)
                table.LoadAllReferences(context);
            foreach (User user in loggenInUsers)
                user.LoadAllReferences(context);
            roomInfo.Players.Player = loggenInUsers.Select(p => new Player()
            {
                Name = p.Name,
                //Table = p.CurrentTable != null ? p.CurrentTable.TableID : 0,
                //Seat = p.CurrentTable != null ? (p.CurrentTable.Table.Seat1 == p.ID ? 1 : p.CurrentTable.Table.Seat2 == p.ID ? 2 : 0) : 0
            }).ToArray();
            roomInfo.Tables.Table = tables.Select(p => new DTO.Objects.Table()
            {
                Number = p.Number,
                BoardSize = p.BoardSize,
                TimeForPlayer = p.TimeForPlayer,
                Seat1 = p.Player1 != null ? p.Player1.Name : string.Empty,
                Seat2 = p.Player2 != null ? p.Player2.Name : string.Empty
            }).ToArray();
            return roomInfo;
        }

        private ResponseStatus GetResponseStatus()
        {
            if (!accessGranted)
                return ResponseStatus.Denied;
            if (userToken == null)
                return ResponseStatus.Error;
            return ResponseStatus.OK;
        }
    }
}
