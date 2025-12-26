namespace BankKata.Core;

public class AccountService
{
    private readonly TextWriter _textWriter;
    private readonly TimeProvider _timeProvider;
    private readonly ITransactionRepository _transactionRepository;

    public AccountService(TextWriter textWriter, TimeProvider timeProvider, ITransactionRepository transactionRepository)
    {
        _textWriter = textWriter;
        _timeProvider = timeProvider;
        _transactionRepository = transactionRepository;
    }

    public async Task Deposit(int amount)
    {
        await _transactionRepository.Add(new Transaction { Id = Guid.NewGuid(), TimeStamp = _timeProvider.GetUtcNow(), Amount = amount });
    }

    public async Task Withdraw(int amount)
    {
        await _transactionRepository.Add(new Transaction { Id = Guid.NewGuid(), TimeStamp = _timeProvider.GetUtcNow(), Amount = -amount });
    }

    public async Task PrintStatement()
    {
        await _textWriter.WriteAsync(StatementGenerator.GetStatement(await _transactionRepository.GetAll()));
    }
}