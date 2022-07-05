using Hexx.Components.Levels;
using Hexx.Connection;
using Hexx.DTO;
using Hexx.DTO.Responses;
using Hexx.Engine;
using Hexx.Engine.Types;

namespace Hexx.Connection.Processors
{
    class RegisterResponseProcessor : ResponseProcessor<RegisterResponse>
    {
        private string OKMessage = "Registration successful. Now you can log in.";
        public RegisterResponseProcessor(RegisterResponse response) : base(response)
        {
            DeniedMessage = "Username already exists! Try another.";
        }

        public override void Process()
        {
            if (Response.Status == ResponseStatus.OK)
            {
                Level level = LevelManager.GetInstance().GetCurrentLevel();
                MessageBoxService.GetInstance(level).Show(OKMessage, MessageBoxButtons.OK, RegistrationSuccessfulMessage, true);
            }
            else
                base.Process();
        }

        private void RegistrationSuccessfulMessage(MessageBoxResult result)
        {
            ConnectionManager.GetInstance().Disconnect();
            Level currentLevel = LevelManager.GetInstance().GetCurrentLevel();
            LevelManager.GetInstance().ChangeLevel<MainMenu>();
        }
    }
}
