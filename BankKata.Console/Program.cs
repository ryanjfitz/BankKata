using BankKata.Core;

var accountService = new AccountService(Console.Out, TimeProvider.System, new InMemoryTransactionRepository());

await accountService.Deposit(1000);

await accountService.Deposit(10);

await accountService.Withdraw(16);

await accountService.PrintStatement();