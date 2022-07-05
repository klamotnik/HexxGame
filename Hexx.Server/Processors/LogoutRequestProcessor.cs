using Hexx.DTO;
using Hexx.DTO.Notifications;
using Hexx.DTO.Objects;
using Hexx.DTO.Requests;
using Hexx.DTO.Responses;
using Hexx.Server.Db;
using Hexx.Server.Db.Entity;
using Hexx.Server.Types;
using System.Collections.Generic;
using System.Linq;

namespace Hexx.Server.Processors
{
    class LogoutRequestProcessor : RequestProcessor<LogoutRequest>
    {
        private bool accessGranted;
        private UserToken userToken;
        private bool error;

        public LogoutRequestProcessor(DTO.Objects.Hexx request) : base(request)
        {
        }

        public override Response Process()
        {
            try
            {
                UserTokenManager tokenManager = new UserTokenManager(context);
                accessGranted = tokenManager.Verify(request.Auth);
                if (accessGranted)
                    Logout();
            }
            catch
            {
                error = true;
            }
            return GenerateResponse(GetResponseStatus());
        }

        protected override Response GenerateResponse(ResponseStatus status)
        {
            return new LogoutResponse()
            {
                Status = status
            };
        }

        private UserToken GetUserToken()
        {
            if (request.Auth == null)
                return null;
            UserToken token = (from t in context.UserTokens where t.ID == request.Auth.UserID select t).FirstOrDefault();
            return token;
        }

        private void VerifyToken(ref UserToken token)
        {
            try
            {
                TokenVerifier verifier = new TokenVerifier(request.Auth, token);
                accessGranted = verifier.Verify();
            }
            catch
            {
                error = true;
            }
        }

        private void Logout()
        {
            UserToken userToken = context.UserTokens.SingleOrDefault(p => p.ID == request.Auth.UserID && p.Token == request.Auth.Token);
            context.UserTokens.Remove(userToken);
            context.SaveChanges();
        }

        private ResponseStatus GetResponseStatus()
        {
            if (!accessGranted)
                return ResponseStatus.Denied;
            if (error)
                return ResponseStatus.Error;
            return ResponseStatus.OK;
        }
    }
}
