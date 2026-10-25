using Dapper;
using School.ViewModel.Common;
using System.Data;

namespace School.Infrastructure.Persistence.Dapper;

public class DapperExecutor(IDbConnectionFactory _dbConnectionFactory)
    : IDapperExecutor
{
    public async Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure,
        object? parameters = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null)
    {
        if (transaction is not null)
        {
            IDbConnection transactionConnection = transaction.Connection
                ?? throw new InvalidOperationException(Universal.DapperTransactionErrorMessage);

            return await transactionConnection.QueryAsync<T>(
                storedProcedure,
                parameters,
                transaction,
                commandType: CommandType.StoredProcedure);
        }

        using IDbConnection connection = _dbConnectionFactory.CreateConnection();

        return await connection.QueryAsync<T>(
            storedProcedure,
            parameters,
            transaction,
            commandTimeout: commandTimeout,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<T?> QuerySingleAsync<T>(string storedProcedure,
        object? parameters = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null)
    {
        if (transaction is not null)
        {
            IDbConnection transactionConnection = transaction.Connection
                ?? throw new InvalidOperationException(Universal.DapperTransactionErrorMessage);

            return await transactionConnection.QuerySingleOrDefaultAsync<T>(
                storedProcedure,
                parameters,
                transaction,
                commandTimeout: commandTimeout,
                commandType: CommandType.StoredProcedure);
        }

        using IDbConnection connection = _dbConnectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<T>(
            storedProcedure,
            parameters,
            transaction,
            commandTimeout: commandTimeout,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> ExecuteAsync(string storedProcedure,
        object? parameters = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null)
    {
        if (transaction is not null)
        {
            IDbConnection transactionConnection = transaction.Connection
                ?? throw new InvalidOperationException(Universal.DapperTransactionErrorMessage);

            return await transactionConnection.ExecuteAsync(
                storedProcedure,
                parameters,
                transaction,
                commandTimeout: commandTimeout,
                commandType: CommandType.StoredProcedure);
        }

        using IDbConnection connection = _dbConnectionFactory.CreateConnection();

        return await connection.ExecuteAsync(
            storedProcedure,
            parameters,
            transaction,
            commandTimeout: commandTimeout,
            commandType: CommandType.StoredProcedure);
    }
}
