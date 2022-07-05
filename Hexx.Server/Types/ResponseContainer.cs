using System.Collections.Generic;
using Hexx.Types;
using Hexx.DTO;

namespace Hexx.Server.Types
{
    public class ResponseContainer : Singleton<ResponseContainer>
    {
        public class ServerResponse
        {
            public ClientManager.Client Client { get; private set; }
            public Response Response { get; private set; }

            public ServerResponse(ClientManager.Client client, Response response)
            {
                Client = client;
                Response = response;
            }
        }

        private List<ServerResponse> serverResponseList = new List<ServerResponse>();

        public void AddResponse(ServerResponse response)
        {
            lock (serverResponseList)
                serverResponseList.Add(response);
        }

        public void AddResponse(ClientManager.Client client, Response response)
        {
            ServerResponse serverResponse = new ServerResponse(client, response);
            AddResponse(serverResponse);
        }

        public IEnumerable<ServerResponse> GetPendingResponses()
        {
            ServerResponse[] serverResponses;
            lock (serverResponseList)
            {
                serverResponses = serverResponseList.ToArray();
                serverResponseList.Clear();
            }
            return serverResponses;
        }
    }
}
