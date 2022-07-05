using Hexx.Components.Levels;
using Hexx.Connection;
using Hexx.DTO;
using Hexx.DTO.Responses;
using Hexx.Engine;

namespace Hexx.Connection.Processors
{
    class LeaveSeatResponseProcessor : ResponseProcessor<LeaveSeatResponse>
    {
        public LeaveSeatResponseProcessor(LeaveSeatResponse response) : base(response)
        {
        }

        public override void Process()
        {
            base.Process();
            if (Response.Status != ResponseStatus.OK)
                return;
            Login.GetInstance().UpdateToken(Response.Auth.Token);
        }
    }
}
