namespace BankKata.Core;

public interface ITransactionRepository
{
    Task Add(Transaction transaction);
    Task<IReadOnlyList<Transaction>> GetAll();
}