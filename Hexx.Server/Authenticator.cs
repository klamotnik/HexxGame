using System;
using System.Linq;
using Hexx.DTO.Requests;
using Hexx.Server.Db;
using Hexx.Server.Db.Entity;

namespace Hexx.Server
{
    public class Authenticator
    {
        private string login;
        private string password;
        private int userID;
        private User user;
        private DatabaseContext context;

        public Authenticator(LoginRequest request) : this(request.Login, request.Password)
        {
        }

        public Authenticator(string login, string password)
        {
            this.login = login;
            this.password = password;
            userID = -1;
            context = new DatabaseContext();
        }

        public bool CheckCredentials()
        {
            user = context.Users.SingleOrDefault(p => p.Name == login && p.Password == Convert.FromBase64String(password));
            if (user == null)
                return false;
            userID = user.ID;
            return true;
        }

        public int GetUserID()
        {
            if (userID == -1)
                throw new Exception("Login failed.");
            return userID;
        }

        public User GetUser()
        {
            if (userID == -1)
                throw new Exception("Login failed.");
            return user;
        }
    }
}
