namespace BankKata.Core;

public class InMemoryTransactionRepository : ITransactionRepository
{
    private readonly List<Transaction> _transactions = [];

    public Task Add(Transaction transaction)
    {
        _transactions.Add(transaction);

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Transaction>> GetAll()
    {
        return Task.FromResult<IReadOnlyList<Transaction>>(_transactions);
    }
}