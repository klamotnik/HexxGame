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
    class CreateTableRequestProcessor : RequestProcessor<CreateTableRequest>
    {
        private UserToken userToken;
        private Table dbTable;
        private UserOnTable userOnTable;
        private bool accessGranted;

        public CreateTableRequestProcessor(DTO.Objects.Hexx request) : base(request)
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
                CreateTable();
            }
            Response response = null;
            try
            {
                context.SaveChanges();
                response = GenerateResponse(GetResponseStatus());
            }
            catch(Exception)
            {
                response = GenerateResponse(ResponseStatus.Error);
            }
            return response;
        }

        private void CreateTable()
        {
            dbTable = new Table();
            dbTable.BoardSize = request.Table.BoardSize;
            dbTable.TimeForPlayer = request.Table.TimeForPlayer;
            //dbTable.GameID = 0;
            context.Add(dbTable);

            userOnTable = new UserOnTable();
            userOnTable.Table = dbTable;
            userOnTable.UserID = request.Auth.UserID;
            context.Add(userOnTable);
        }
        
        protected override Response GenerateResponse(ResponseStatus status)
        {
            CreateTableResponse response = new CreateTableResponse();
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
                        Seat1 = null,
                        Seat2 = null,
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
