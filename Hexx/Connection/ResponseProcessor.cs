using Hexx.Components.Levels;
using Hexx.DTO;
using Hexx.DTO.Responses;
using Hexx.Engine;
using Hexx.Engine.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Connection
{
    public abstract class ResponseProcessor<T> : ResponseProcessor where T : Response
    {
        public new T Response => (T)base.Response;
        
        protected ResponseProcessor(T response) : base(response)
        {
        }
    }

    public abstract class ResponseProcessor
    {
        public Response Response { get; private set; }
        public event MessageBoxCallback OnDeniedMessageBoxOK;
        public event MessageBoxCallback OnErrorMessageBoxOK;
        protected string DeniedMessage = "Session expired. Please iog in again.";
        protected string ErrorMessage = "Internal server error. Please try again later.";

        public static ResponseProcessor GetProcessor(Response response)
        {
            Type processorType = Type.GetType(typeof(ResponseProcessor).Namespace + ".Processors." + response.GetType().Name + "Processor, Hexx");
            if (processorType == null)
                throw new NotSupportedException(response.GetType().Name + " is not supported.");
            return Activator.CreateInstance(processorType, new object[] { response }) as ResponseProcessor;
        } 

        protected ResponseProcessor(Response response)
        {
            Response = response;
        }

        public virtual void Process()
        {
            Level level = LevelManager.GetInstance().GetCurrentLevel();
            if (Response.Status == ResponseStatus.Denied)
                MessageBoxService.GetInstance(level).Show(DeniedMessage, MessageBoxButtons.OK, SessionExpiredMessage, true);
            else if (Response.Status == ResponseStatus.Error)
                MessageBoxService.GetInstance(level).Show(ErrorMessage, MessageBoxButtons.OK, OnErrorMessageBoxOK, true);
        }
        
        private void SessionExpiredMessage(MessageBoxResult result)
        {
            ConnectionManager.GetInstance().Disconnect();
            Level currentLevel = LevelManager.GetInstance().GetCurrentLevel();
            if (!(currentLevel is MainMenu))
            {
                Login.GetInstance().Logout();
                LevelManager.GetInstance().ChangeLevel<MainMenu>();
            }
            OnDeniedMessageBoxOK?.Invoke(result);
        }
    }
}
