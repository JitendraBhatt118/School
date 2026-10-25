using System.Data;

namespace School.Infrastructure.Persistence.Dapper;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
