using System;
using System.Linq;
using Hexx.Server.Db;
using Hexx.Server.Types;
using Hexx.DTO;
using Hexx.DTO.Objects;
using Hexx.DTO.Requests;
using Hexx.DTO.Responses;
using Hexx.Server.Db.Entity;

namespace Hexx.Server.Processors
{
    class LeaveTableRequestProcessor : RequestProcessor<LeaveTableRequest>
    {
        private UserToken userToken;
        private bool accessGranted;
        private bool error;

        public LeaveTableRequestProcessor(DTO.Objects.Hexx request) : base(request)
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
                LeaveTable();
            }
            Response response = null; 
            try
            {
                context.SaveChanges();
                response = GenerateResponse(GetResponseStatus());
            }
            catch
            {
                response = GenerateResponse(ResponseStatus.Error);
            }
            return response;
        }

        private void LeaveTable()
        {
            UserOnTable userOnTable = context.UsersOnTable.SingleOrDefault(p => p.UserID == request.Auth.UserID);
            userOnTable.LoadAllReferences(context);
            if(userOnTable == null)
            {
                error = true;
                return;
            }
            if (context.UsersOnTable.Count(p=>p.Table == userOnTable.Table) == 1)
                context.Remove(userOnTable.Table);
            else
            {
                if (userOnTable.Table.Seat1 == request.Auth.UserID)
                    userOnTable.Table.Seat1 = 0;
                if (userOnTable.Table.Seat2 == request.Auth.UserID)
                    userOnTable.Table.Seat2 = 0;
            }
            context.UsersOnTable.Remove(userOnTable);
        }

        protected override Response GenerateResponse(ResponseStatus status)
        {
            LeaveTableResponse response = new LeaveTableResponse();
            response.Status = status;
            if (status == ResponseStatus.OK || status == ResponseStatus.Error)
            {
                response.Auth = new Auth();
                response.Auth.UserID = userToken.ID;
                response.Auth.Token = userToken.Token;
            }
            return response;
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
