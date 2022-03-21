using System.Data;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;

namespace Mersani.Utility
{
    public class Database
    {
        private readonly IConfiguration configuration;
        public Database(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public IDbConnection GetConnection()
        {
            var connectionString = configuration.GetSection("ConnectionStrings").GetSection("OrcleStr").Value;
            var conn = new OracleConnection(connectionString);
            return conn;
        }
    }
}
