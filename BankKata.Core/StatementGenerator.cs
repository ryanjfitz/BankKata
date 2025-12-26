using System.Text;

namespace BankKata.Core;

public static class StatementGenerator
{
    public static string GetStatement(IEnumerable<Transaction> transactions)
    {
        var statement = new StringBuilder();

        statement.Append("Date | Amount | Balance");

        foreach (var line in GetLines(transactions))
        {
            statement.AppendLine();
            statement.Append(line);
        }

        return statement.ToString();
    }

    private static IEnumerable<string> GetLines(IEnumerable<Transaction> transactions)
    {
        int runningBalance = 0;

        LinkedList<string> lines = [];

        foreach (var transaction in transactions.OrderBy(t => t.TimeStamp))
        {
            runningBalance += transaction.Amount;
            lines.AddFirst($"{transaction.TimeStamp:MM/dd/yyyy} | {transaction.Amount} | {runningBalance}");
        }

        return lines;
    }
}