using AwesomeAssertions;
using BankKata.Core;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;

namespace BankKata.Infrastructure.Tests;

public class SqlServerTransactionRepositoryTests
{
    private readonly MsSqlContainer _msSqlContainer;
    private readonly SqlConnection _sqlConnection;
    private readonly SqlServerTransactionRepository _sut;

    public SqlServerTransactionRepositoryTests()
    {
        _msSqlContainer = new MsSqlBuilder().Build();
        _sqlConnection = new SqlConnection();
        _sut = new SqlServerTransactionRepository(_sqlConnection);
    }

    [Before(Test)]
    public async Task Before()
    {
        await _msSqlContainer.StartAsync();
        _sqlConnection.ConnectionString = _msSqlContainer.GetConnectionString();
        await _sqlConnection.OpenAsync();
        await _sut.CreateTransactionsTable();
    }

    [Test]
    public async Task Should_insert_record()
    {
        var transaction = new Transaction { Id = Guid.NewGuid(), TimeStamp = DateTimeOffset.UtcNow, Amount = 100 };

        await _sut.Add(transaction);

        (await _sut.GetAll()).Should().ContainSingle().Which.Should().BeEquivalentTo(transaction);
    }

    [After(Test)]
    public async Task After()
    {
        await _sqlConnection.DisposeAsync();
        await _msSqlContainer.StopAsync();
        await _msSqlContainer.DisposeAsync();
    }
}