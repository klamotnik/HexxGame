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
    [Processor(typeof(RegisterRequest))]
    class RegisterRequestProcessor : RequestProcessor<RegisterRequest>
    {
        public RegisterRequestProcessor(DTO.Objects.Hexx request) : base(request)
        {
        }

        public override Response Process()
        {
            ResponseStatus status = ResponseStatus.Denied;
            if(!UsernameExists())
            {
                User user = new User();
                user.Name = request.Login;
                user.Password = Convert.FromBase64String(request.Password);
                user.Mail = "";
                context.Add(user);
                try
                {
                    context.SaveChanges();
                    status = ResponseStatus.OK;
                }
                catch(Exception ex)
                {
                    status = ResponseStatus.Error;
                }
            }
            Response response = GenerateResponse(status);
            return response;
        }

        private bool UsernameExists()
        {
            return context.Users.Any(p => p.Name == request.Login);
        }

        protected override Response GenerateResponse(ResponseStatus status)
        {
            RegisterResponse response = new RegisterResponse();
            response.Status = status;
            return response;
        }
    }
}
