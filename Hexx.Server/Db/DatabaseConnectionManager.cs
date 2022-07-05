using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Security;
using Hexx.Server.Types;
using Hexx.Types;

namespace Hexx.Server.Db
{
    public class DatabaseConnectionManager : Singleton<DatabaseConnectionManager>
    {
        private string address;
        private string databaseName;
        private string login;
        private SecureString password;
        private bool integratedLogin;

        public void Initialize(DatabaseConnectionData connectionData)
        {
            Initialize(connectionData.Address, connectionData.DatabaseName, connectionData.Login, connectionData.Password, connectionData.IntegratedLogin);
        }

        public void Initialize(string address, string databaseName, string login, string password, bool integratedLogin)
        {
            Initialize(address, databaseName, login, StringToSecureString(password), integratedLogin);
        }

        public void Initialize(string address, string databaseName, string login, SecureString password, bool integratedLogin)
        {
            if (string.IsNullOrEmpty(this.address) || string.IsNullOrEmpty(this.databaseName) || (!this.integratedLogin && (string.IsNullOrEmpty(this.login) || password == null)))
            {
                if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(databaseName) || (!integratedLogin && (string.IsNullOrEmpty(login) || password == null)))
                    throw new Exception("Data in parameters are incomplete.");
                this.address = address;
                this.databaseName = databaseName;
                this.login = login;
                this.password = password;
                this.integratedLogin = integratedLogin;
            }
            else
                throw new Exception("Database configuration can be initialized only once!");
        }

        public bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = GetSqlConnection())
                {
                    connection.Open();
                    connection.Close();
                }
            }
            catch (SqlException)
            {
                return false;
            }
            return true;
        }

        public string GetConnectionString()
        {
            if(integratedLogin)
                return string.Format("Persist Security Info=False;Integrated Security=true;Data Source={0};Initial Catalog={1};MultipleActiveResultSets=true;", address, databaseName);
            return string.Format("Data Source={0};Initial Catalog={1};User id={2};Password={3};MultipleActiveResultSets=true", address, databaseName, login, SecureStringToString(password));
        }

        public SqlConnection GetSqlConnection()
        {
            return new SqlConnection(GetConnectionString());
        }

        private string SecureStringToString(SecureString value)
        {
            IntPtr bstr = Marshal.SecureStringToBSTR(value);
            try
            {
                return Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                Marshal.FreeBSTR(bstr);
            }
        }

        private SecureString StringToSecureString(string value)
        {
            SecureString secureString = new SecureString();
            foreach (char c in value)
                secureString.AppendChar(c);
            return secureString;
        }
    }
}
