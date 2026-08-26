using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Data
{
    public class DBHelper
    {
        private readonly string _connectionString;

        public DBHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("GenxAIDB")
                ?? throw new InvalidOperationException("Connection string 'GenxAIDB' not found.");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
