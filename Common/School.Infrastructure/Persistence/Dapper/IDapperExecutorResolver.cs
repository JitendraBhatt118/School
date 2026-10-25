namespace School.Infrastructure.Persistence.Dapper;

public interface IDapperExecutorResolver
{
    IDapperExecutor Get(string connectionName);
}
