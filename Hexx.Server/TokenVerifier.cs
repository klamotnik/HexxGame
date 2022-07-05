using System.Linq;
using Hexx.DTO.Objects;
using Hexx.Server.Db;
using Hexx.Server.Db.Entity;
using Hexx.Types.Interfaces;

namespace Hexx.Server
{
    public class TokenVerifier : IVerifier
    {
        private Auth data;
        private UserToken token;

        public TokenVerifier(Auth authorizationData)
        {
            data = authorizationData;
        }

        public TokenVerifier(Auth authorizationData, UserToken userToken) : this(authorizationData)
        {
            token = userToken;
        }

        public bool Verify()
        {
            if (token == null)
            {
                DatabaseConnectionManager connectionManager = DatabaseConnectionManager.GetInstance();
                DatabaseContext context = new DatabaseContext();
                token = (from token in context.UserTokens where token.ID == data.UserID select token).FirstOrDefault();
            }
            return token?.Token == data.Token;
        }
    }
}
