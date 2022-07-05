using System;
using System.Collections.Generic;
using Hexx.DTO;
using Hexx.DTO.Requests;
using Hexx.Server.Generators;
using Hexx.Server.Processors;
using Hexx.Server.Types;

namespace Hexx.Server
{
    public class NotificationGeneratorFactory
    {
        public static IEnumerable<NotificationGenerator> GetGeneratorsByRequestResponse(Request request, Response response)
        {
            List<NotificationGenerator> generators = new List<NotificationGenerator>();
            generators.Add(new PlayerChangeNotificationGenerator(request, response));
            generators.Add(new TableChangeNotificationGenerator(request, response));
            return generators;
        }
    }
}
