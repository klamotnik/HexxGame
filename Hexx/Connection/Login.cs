using Hexx.DTO;
using Hexx.DTO.Objects;
using Hexx.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Connection
{
    public class Login : Singleton<Login>
    {
        public string Username { get; }
        public Table CurrentTable { get; private set; }
        private string token;
        private int userID;

        public static Login SignIn(int userID, string username, string token)
        {
            if (instance != null)
                throw new NotSupportedException("You are logged in. Please Log out and try again.");
            instance = new Login(userID, username, token);
            return instance;
        }

        public bool Logout()
        {
            if (instance == null)
                return false;

            instance = null;
            return true;
        }

        public bool UpdateToken(string token)
        {
            this.token = token;
            return true;
        }

        public void UpdateTableInfo(Table table)
        {
            CurrentTable = table;
        }

        public Auth GetAuth()
        {
            return new Auth()
            {
                UserID = userID,
                Token = token
            };
        }

        private Login(int userID, string username, string token) : base(true)
        {
            this.userID = userID;
            Username = username;
            this.token = token;
        }

        private new static Login GetInstance()
        {
            if(instance == null)
                throw new NotSupportedException("You are not logged in!");
            return Singleton<Login>.GetInstance();
        }

        private static DTO.Objects.Hexx WrapRequest(Request request)
        {
            DTO.Objects.Hexx serverResponse = new DTO.Objects.Hexx()
            {
                Action = request,
                Type = (ActionType)Enum.Parse(typeof(ActionType), request.GetType().Name.ToString())
            };
            return serverResponse;
        }
    }
}
