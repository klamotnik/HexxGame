using Hexx.Components.Levels;
using Hexx.Connection;
using Hexx.DTO;
using Hexx.DTO.Responses;
using Hexx.Engine;

namespace Hexx.Connection.Processors
{
    class LogoutResponseProcessor : ResponseProcessor<LogoutResponse>
    {
        public LogoutResponseProcessor(LogoutResponse response) : base(response)
        {
        }

        public override void Process()
        {
            base.Process();
            if (Response.Status != ResponseStatus.OK)
                return;
            Login.GetInstance().Logout();
            LevelManager.GetInstance().ChangeLevel<MainMenu>();
            ConnectionManager.GetInstance().Disconnect();
        }
    }
}
