using System.Security;

namespace Hexx.Server.Types
{
    public class DatabaseConnectionData
    {
        public string Address { get; set; }
        public string DatabaseName { get; set; }
        public string Login { get; set; }
        public SecureString Password { get; set; }
        public bool IntegratedLogin { get; set; }
    }
}
