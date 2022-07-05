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
    class StartGameRequestProcessor : RequestProcessor<StartGameRequest>
    {
        private UserToken userToken;
        private Table dbTable;
        private UserOnTable userOnTable;
        private bool accessGranted;

        public StartGameRequestProcessor(DTO.Objects.Hexx request) : base(request)
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
                StartGame();
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

        private void StartGame()
        {
            
            context.Update(dbTable);
        }

        protected override Response GenerateResponse(ResponseStatus status)
        {
            StartGameResponse response = new StartGameResponse();
            response.Status = status;
            if (status == ResponseStatus.OK || status == ResponseStatus.Error)
            {
                response.Auth = new DTO.Objects.Auth();
                response.Auth.UserID = userToken.ID;
                response.Auth.Token = userToken.Token;
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
