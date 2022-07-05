using System;
using System.Linq;
using Hexx.Server.Types;
using Hexx.Server.Db;
using Hexx.DTO.Objects;
using Hexx.DTO;
using Hexx.DTO.Requests;
using Hexx.DTO.Responses;
using Hexx.DTO.Notifications;
using System.Collections.Generic;
using Hexx.Server.Db.Entity;
using Hexx.Types.Attributes;

namespace Hexx.Server.Processors
{
    [Processor(typeof(LoginRequest))]
    class LoginRequestProcessor : RequestProcessor<LoginRequest>
    {
        private ClientManager.Client client;
        private UserToken userToken;
        private bool accessGranted;
        private Authenticator authenticator;

        public LoginRequestProcessor(DTO.Objects.Hexx request, ClientManager.Client client) : base(request)
        {
            this.client = client;
        }

        public override Response Process()
        {
            authenticator = new Authenticator(request);
            accessGranted = authenticator.CheckCredentials();
            if (accessGranted)
                Login(authenticator.GetUserID());
            Response response = GenerateResponse(GetResponseStatus());
            return response;
        }

        protected override Response GenerateResponse(ResponseStatus status)
        {
            LoginResponse response = new LoginResponse();
            response.Status = status;
            if (status == ResponseStatus.OK)
            {
                userToken.LoadAllReferences(context);
                response.Auth = new Auth();
                response.Auth.UserID = userToken.ID;
                response.Auth.Token = userToken.Token;
                response.Username = userToken.User.Name;
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

        private void Login(int userID)
        {
            UserTokenManager tokenManager = new UserTokenManager(context);
            userToken = tokenManager.GenerateNewUserToken(userID, client.Guid.ToString("N"));
            userToken.ClientGuid = client.Guid.ToString("N");
            //context.Update(userToken);
            try
            {
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                userToken = null;
            }
        }
    }
}
