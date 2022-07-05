using Hexx.Components.Levels;
using Hexx.Connection;
using Hexx.DTO;
using Hexx.DTO.Responses;
using Hexx.Engine;

namespace Hexx.Connection.Processors
{
    class CreateTableResponseProcessor : ResponseProcessor<CreateTableResponse>
    {
        public CreateTableResponseProcessor(CreateTableResponse response) : base(response)
        {
        }

        public override void Process()
        {
            base.Process();
            if (Response.Status != ResponseStatus.OK)
                return;
            Login login = Login.GetInstance();
            login.UpdateToken(Response.Auth.Token);
            login.UpdateTableInfo(Response.Table);
            LevelManager.GetInstance().ChangeLevel<GameTable>();
        }
    }
}
