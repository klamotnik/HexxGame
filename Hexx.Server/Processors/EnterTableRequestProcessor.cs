using System;
using System.Linq;
using Hexx.Server.Db;
using Hexx.Server.Types;
using Hexx.DTO;
using Hexx.DTO.Requests;
using Hexx.DTO.Responses;
using Hexx.Server.Db.Entity;

namespace Hexx.Server.Processors
{
    class EnterTableRequestProcessor : RequestProcessor<EnterTableRequest>
    {
        private UserToken userToken;
        private Table dbTable;
        private UserOnTable userOnTable;
        private bool accessGranted;

        public EnterTableRequestProcessor(DTO.Objects.Hexx request) : base(request)
        {
        }

        public override Response Process()
        {
            UserTokenManager tokenManager = new UserTokenManager(context);
            accessGranted = tokenManager.Verify(request.Auth);
            if (accessGranted)
            {
                tokenManager.UpdateToken();
                userToken = tokenManager.userToken;
                EnterTable();
            }
            Response response = null;
            try
            {
                context.SaveChanges();
                response = GenerateResponse(GetResponseStatus());
            }
            catch (Exception ex)
            {
                response = GenerateResponse(ResponseStatus.Error);
            }
            return response;
        }

        private void EnterTable()
        {
            dbTable = context.Tables.SingleOrDefault(p => p.Number == request.TableNumber);
            if (dbTable == null)
                return;
            userOnTable = new UserOnTable();
            userOnTable.Table = dbTable;
            userOnTable.UserID = request.Auth.UserID;
            context.Add(userOnTable);
        }

        protected override Response GenerateResponse(ResponseStatus status)
        {
            EnterTableResponse response = new EnterTableResponse();
            response.Status = status;
            if (status == ResponseStatus.OK || status == ResponseStatus.Error)
            {
                response.Auth = new DTO.Objects.Auth();
                response.Auth.UserID = userToken.ID;
                response.Auth.Token = userToken.Token;
                if (status == ResponseStatus.OK)
                {
                    response.Table = new DTO.Objects.Table()
                    {
                        BoardSize = dbTable.BoardSize,
                        Number = dbTable.Number,
                        Seat1 = dbTable.Player1?.Name,
                        Seat2 = dbTable.Player2?.Name,
                        TimeForPlayer = dbTable.TimeForPlayer
                    };
                }
            }
            return response;
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
