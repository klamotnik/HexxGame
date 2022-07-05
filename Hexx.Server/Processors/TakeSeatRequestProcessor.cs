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
    class TakeSeatRequestProcessor : RequestProcessor<TakeSeatRequest>
    {
        private UserToken userToken;
        private Table dbTable;
        private UserOnTable userOnTable;
        private bool accessGranted;

        public TakeSeatRequestProcessor(DTO.Objects.Hexx request) : base(request)
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
                TakeSeat();
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

        private void TakeSeat()
        {
            dbTable = context.Tables.SingleOrDefault(p => p.Number == request.TableNumber);
            if (dbTable == null)
                return;
            if (request.Seat == 1)
                dbTable.Seat1 = userToken.ID;
            else if (request.Seat == 2)
                dbTable.Seat2 = userToken.ID;
            context.Update(dbTable);
        }

        protected override Response GenerateResponse(ResponseStatus status)
        {
            TakeSeatResponse response = new TakeSeatResponse();
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
