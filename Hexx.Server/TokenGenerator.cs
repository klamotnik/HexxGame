using System;

namespace Hexx.Server
{
    public class TokenGenerator
    {
        private TokenGenerator()
        {

        }

        public static string Generate()
        {
            return Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
        }
    }
}
