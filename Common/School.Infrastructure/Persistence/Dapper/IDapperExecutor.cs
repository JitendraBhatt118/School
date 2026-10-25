using System.Data;

namespace School.Infrastructure.Persistence.Dapper;

public interface IDapperExecutor
{
    Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure,
        object? parameters = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null);

    Task<T?> QuerySingleAsync<T>(string storedProcedure,
        object? parameters = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null);

    Task<int> ExecuteAsync(string storedProcedure,
        object? parameters = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null);
}
