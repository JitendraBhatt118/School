using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace School.Infrastructure.Persistence.Dapper;

public class SqlConnectionFactory(IConfiguration _configuration,
    string _connectionStringName)
    : IDbConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_configuration.GetConnectionString(_connectionStringName));
    }
}
