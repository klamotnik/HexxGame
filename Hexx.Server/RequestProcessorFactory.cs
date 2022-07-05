using System;
using Hexx.DTO.Requests;
using Hexx.Server.Processors;
using Hexx.Server.Types;

namespace Hexx.Server
{
    public class RequestProcessorFactory
    {
        public static RequestProcessor GetProcessor(DTO.Objects.Hexx request, ClientManager.Client client)
        {
            if (request.Action == null)
                throw new Exception("Request action is not set.");
            if (request.Action is LoginRequest)
                return new LoginRequestProcessor(request, client);
            else
            {
                Type processorType = Type.GetType($"Hexx.Server.Processors.{request.Type}Processor, Hexx.Server");
                if(processorType == null)
                    throw new Exception(request.Type.ToString() + " is not supported.");
                return Activator.CreateInstance(processorType, new object[] { request }) as RequestProcessor;
            }
        }
    }
}
