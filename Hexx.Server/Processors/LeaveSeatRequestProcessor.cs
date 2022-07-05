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
    class LeaveSeatRequestProcessor : RequestProcessor<LeaveSeatRequest>
    {
        private UserToken userToken;
        private Table dbTable;
        private UserOnTable userOnTable;
        private bool accessGranted;

        public LeaveSeatRequestProcessor(DTO.Objects.Hexx request) : base(request)
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
                LeaveSeat();
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

        private void LeaveSeat()
        {
            User user = context.Users.Where(p => p.ID == request.Auth.UserID).FirstOrDefault();
            user.LoadAllReferences(context);
            user.CurrentTable.LoadAllReferences(context);
            dbTable = user.CurrentTable.Table;
            if (dbTable == null)
                return;
            if (dbTable.Seat1 == request.Auth.UserID)
                dbTable.Seat1 = null;
            if (dbTable.Seat2 == request.Auth.UserID)
                dbTable.Seat2 = null;
            context.Update(dbTable);
        }

        protected override Response GenerateResponse(ResponseStatus status)
        {
            LeaveSeatResponse response = new LeaveSeatResponse();
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
