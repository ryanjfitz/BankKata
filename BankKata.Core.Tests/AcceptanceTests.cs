using AwesomeAssertions;
using Microsoft.Extensions.Time.Testing;

namespace BankKata.Core.Tests;

public class AcceptanceTests : IDisposable
{
    private readonly StringWriter _stringWriter;
    private readonly FakeTimeProvider _fakeTimeProvider;
    private readonly InMemoryTransactionRepository _inMemoryTransactionRepository;
    private readonly AccountService _sut;

    public AcceptanceTests()
    {
        _stringWriter = new StringWriter();
        _fakeTimeProvider = new FakeTimeProvider();
        _inMemoryTransactionRepository = new InMemoryTransactionRepository();
        _sut = new AccountService(_stringWriter, _fakeTimeProvider, _inMemoryTransactionRepository);
    }

    [Test]
    public async Task Should_print_empty_statement()
    {
        await _sut.PrintStatement();

        _stringWriter.ToString().Should().Be("Date | Amount | Balance");
    }

    [Test]
    public async Task Should_print_deposit_on_statement()
    {
        _fakeTimeProvider.SetUtcNow(new DateTime(2025, 1, 1));
        await _sut.Deposit(100);

        await _sut.PrintStatement();

        _stringWriter.ToString().Should().Be("""
                                             Date | Amount | Balance
                                             01/01/2025 | 100 | 100
                                             """);
    }

    [Test]
    public async Task Should_print_withdrawal_on_statement()
    {
        _fakeTimeProvider.SetUtcNow(new DateTime(2025, 6, 15));
        await _sut.Withdraw(1750);

        await _sut.PrintStatement();

        _stringWriter.ToString().Should().Be("""
                                             Date | Amount | Balance
                                             06/15/2025 | -1750 | -1750
                                             """);
    }

    [Test]
    public async Task Should_print_running_balance_on_statement()
    {
        _fakeTimeProvider.SetUtcNow(new DateTime(2012, 1, 10));
        await _sut.Deposit(1000);

        _fakeTimeProvider.SetUtcNow(new DateTime(2012, 1, 13));
        await _sut.Deposit(2000);

        _fakeTimeProvider.SetUtcNow(new DateTime(2012, 1, 14));
        await _sut.Withdraw(500);

        await _sut.PrintStatement();

        _stringWriter.ToString().Should().Be("""
                                             Date | Amount | Balance
                                             01/14/2012 | -500 | 2500
                                             01/13/2012 | 2000 | 3000
                                             01/10/2012 | 1000 | 1000
                                             """);
    }

    [Test]
    public async Task Should_record_deposit_transaction()
    {
        _fakeTimeProvider.SetUtcNow(new DateTime(2025, 1, 1));

        await _sut.Deposit(100);

        var transactions = await _inMemoryTransactionRepository.GetAll();
        transactions.Count.Should().Be(1);
        transactions[0].Id.Should().NotBe(Guid.Empty);
        transactions[0].TimeStamp.Should().Be(new DateTime(2025, 1, 1));
        transactions[0].Amount.Should().Be(100);
    }

    [Test]
    public async Task Should_record_withdrawal_transaction()
    {
        _fakeTimeProvider.SetUtcNow(new DateTime(2025, 6, 15));

        await _sut.Withdraw(1750);

        var transactions = await _inMemoryTransactionRepository.GetAll();
        transactions.Count.Should().Be(1);
        transactions[0].Id.Should().NotBe(Guid.Empty);
        transactions[0].TimeStamp.Should().Be(new DateTime(2025, 6, 15));
        transactions[0].Amount.Should().Be(-1750);
    }

    public void Dispose()
    {
        _stringWriter.Dispose();
    }
}