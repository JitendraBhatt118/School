using Microsoft.Extensions.Configuration;

namespace School.Infrastructure.Persistence.Dapper;

public class DapperExecutorResolver(IConfiguration _configuration)
    : IDapperExecutorResolver
{
    public IDapperExecutor Get(string connectionName)
    {
        SqlConnectionFactory factory = new(_configuration, connectionName);
        return new DapperExecutor(factory);
    }
}
