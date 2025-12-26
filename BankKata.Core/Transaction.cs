namespace BankKata.Core;

public class Transaction
{
    public Guid Id { get; set; }

    public DateTimeOffset TimeStamp { get; set; }

    public int Amount { get; set; }
}