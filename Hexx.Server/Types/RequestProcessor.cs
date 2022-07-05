using Hexx.DTO;
using Hexx.Server.Db;

namespace Hexx.Server.Types
{
    public abstract class RequestProcessor<T> : RequestProcessor where T : Request
    {
        protected T request;
        protected DatabaseContext context;

        public RequestProcessor(T request)
        {
            this.request = request;
            context = new DatabaseContext();
        }

        public RequestProcessor(DTO.Objects.Hexx request) : this(request.Action as T)
        {
        }
    }

    public abstract class RequestProcessor
    {
        public abstract Response Process();
        protected abstract Response GenerateResponse(ResponseStatus status);
    }
}
