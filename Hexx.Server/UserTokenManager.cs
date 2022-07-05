using Hexx.DTO.Objects;
using Hexx.DTO.Requests;
using Hexx.Server.Db;
using Hexx.Server.Db.Entity;
using System;
using System.Linq;

namespace Hexx.Server
{
    public class UserTokenManager
    {
        public UserToken userToken { get; private set; }
        private DatabaseContext context;

        public UserTokenManager(DatabaseContext context)
        {
            this.context = context;
        }

        public UserTokenManager(UserToken userToken, DatabaseContext context)
        {
            this.context = context;
            this.userToken = userToken;
        }

        public UserToken GenerateNewUserToken(int userID, string clientGuid)
        {
            userToken = context.UserTokens.SingleOrDefault(p => p.ID == userID);
            if (userToken != null)
                UpdateToken();
            else
            {
                userToken = new UserToken()
                {
                    ID = userID,
                    Token = GenerateNewToken(),
                    ClientGuid = clientGuid
                };
                context.UserTokens.Add(userToken);
            }
            return userToken;
        }

        public bool UpdateToken()
        {
            if (userToken == null)
                return false;
            userToken.Token = GenerateNewToken();
            context.UserTokens.Update(userToken);
            return true;
        }

        public bool Verify(Auth auth)
        {
            if (userToken == null)
                userToken = context.UserTokens.SingleOrDefault(p => p.ID == auth.UserID);
            return userToken?.Token == auth.Token;
        }
        
        private string GenerateNewToken()
        {
            return Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
        }
    }
}
