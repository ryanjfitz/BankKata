using BankKata.Core;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BankKata.Infrastructure;

public class SqlServerTransactionRepository : ITransactionRepository
{
    private readonly SqlConnection _sqlConnection;

    public SqlServerTransactionRepository(SqlConnection sqlConnection)
    {
        _sqlConnection = sqlConnection;
    }

    public async Task CreateTransactionsTable()
    {
        await _sqlConnection.ExecuteAsync("CREATE TABLE [dbo].[Transactions] (Id UNIQUEIDENTIFIER PRIMARY KEY, TimeStamp DATETIMEOFFSET, Amount INT)");
    }

    public async Task Add(Transaction transaction)
    {
        await _sqlConnection.ExecuteAsync("INSERT INTO [dbo].[Transactions] (Id, TimeStamp, Amount) VALUES (@Id, @TimeStamp, @Amount)",
            new { transaction.Id, transaction.TimeStamp, transaction.Amount });
    }

    public async Task<IReadOnlyList<Transaction>> GetAll()
    {
        return (await _sqlConnection.QueryAsync<Transaction>("SELECT * FROM [dbo].[Transactions]")).AsList();
    }
}