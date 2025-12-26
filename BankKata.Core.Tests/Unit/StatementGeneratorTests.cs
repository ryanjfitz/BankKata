using AwesomeAssertions;

namespace BankKata.Core.Tests.Unit;

public class StatementGeneratorTests
{
    [Test]
    public void Should_print_transactions_in_date_descending_order()
    {
        // Create list of transactions that are out of order by date
        IEnumerable<Transaction> transactions =
        [
            new() { Id = Guid.NewGuid(), TimeStamp = new DateTime(2024, 1, 2), Amount = -200 },
            new() { Id = Guid.NewGuid(), TimeStamp = new DateTime(2024, 1, 1), Amount = 500 }
        ];

        StatementGenerator.GetStatement(transactions).Should().Be("""
                                                                  Date | Amount | Balance
                                                                  01/02/2024 | -200 | 300
                                                                  01/01/2024 | 500 | 500
                                                                  """);
    }
}