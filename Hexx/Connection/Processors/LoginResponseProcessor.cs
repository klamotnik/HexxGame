using Hexx.Components.Levels;
using Hexx.Connection;
using Hexx.DTO;
using Hexx.DTO.Responses;
using Hexx.Engine;

namespace Hexx.Connection.Processors
{
    class LoginResponseProcessor : ResponseProcessor<LoginResponse>
    {
        public LoginResponseProcessor(LoginResponse response) : base(response)
        {
            DeniedMessage = "Username or password is not correct.";
        }

        public override void Process()
        {
            base.Process();
            if (Response.Status != ResponseStatus.OK)
                return;
            Login.SignIn(Response.Auth.UserID, Response.Username, Response.Auth.Token);
            LevelManager.GetInstance().ChangeLevel<Room>();
        }
    }
}
