using Bank.AppLogic.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BankConcurrencyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Test..");

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var services = new ServiceCollection();
            services.AddSingleton(typeof(IConfiguration), configBuilder.Build());
            Bank.AppLogic.Intialization.InitIocContainer(services);
            Bank.Data.Initialization.InitIocContainer(services);
            var serviceProvider = services.BuildServiceProvider();
            
            var transactiontest = new TransactionTest(serviceProvider);

            transactiontest.CreateAccounts();

            var getBal1 = transactiontest.GetAccount("ACTTEST123456");

            var tran1 = transactiontest.Deposit(getBal1.Id, 30);
            var tran2 = transactiontest.Transfer(getBal1.Id, "ACTTEST123457", 10);
            var tran3 = transactiontest.Deposit(getBal1.Id, 100);

            Task.WaitAll(tran1, tran2, tran3);

            var getBal2 = transactiontest.GetAccount("ACTTEST123456");

            Console.WriteLine("Initial User ACCT Test1 Balance: " + getBal1.Balance);
            Console.WriteLine(string.Format("Tran 1 Result: {0}", tran1.Result.IsSuccess ? "Success" : tran1.Result.ErrorCode));
            Console.WriteLine(string.Format("Tran 2 Result: {0}", tran2.Result.IsSuccess ? "Success" : tran2.Result.ErrorCode));
            Console.WriteLine(string.Format("Tran 3 Result: {0}", tran3.Result.IsSuccess ? "Success" : tran3.Result.ErrorCode));
            Console.WriteLine("Latest User ACCT Test1 Balance: " + getBal2.Balance);
            Console.WriteLine(string.Format("Expected User ACCT Test1 Balance: {0}", (getBal1.Balance + 30) - 10 + 100));

            Console.ReadLine();
        }
    }


    public class TransactionTest
    {
        private ITransactionService _transactionService;
        private IAccountService _accountService;
        public TransactionTest(IServiceProvider provider)
        {
            _transactionService = provider.GetService<ITransactionService>();
            _accountService = provider.GetService<IAccountService>();
        }

        public void CreateAccounts()
        {
            _accountService.Create(new Bank.Domain.Account { AccountName = "ACCT Test1", AccountNumber = "ACTTEST123456", Balance = 50, Password = "test123!" });
            _accountService.Create(new Bank.Domain.Account { AccountName = "ACCT Test2", AccountNumber = "ACTTEST123457", Balance = 50, Password = "test123!" });
        }

        public Bank.Domain.Account GetAccount(string accountNumber)
        {
            return _accountService.GetBy(accountNumber);
        }

        public Task<Bank.Domain.TransactionResponse> Deposit(int accountId, decimal amount)
        {
            return Task.Run(() => _transactionService.Deposit(new Bank.Domain.Deposit { AccountId = accountId, Amount = amount }));
        }

        public Task<Bank.Domain.TransactionResponse> Transfer(int accountId, string accoutNumber, decimal amount)
        {
            return Task.Run(() => _transactionService.Transfer(new Bank.Domain.TransferFunds { AccountId = accountId, TransferToAccountNumber = accoutNumber, Amount = amount }));
        }
    }
}
