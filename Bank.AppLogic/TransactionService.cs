using Bank.AppLogic.Interfaces;
using Bank.Domain;
using Bank.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.AppLogic
{
    public class TransactionService : ITransactionService
    {
        private IDatabase _database;
        private IAccountService _accountService;
        public TransactionService(IDatabase database, IAccountService accountService)
        {
            _database = database;
            _accountService = accountService;
        }

        public bool Deposit(Deposit deposit, string transactionInfo = null)
        {
            var acct = _accountService.Get(deposit.AccountId);
            var newBalance = acct.Balance + deposit.Amount;

            var query = @"UPDATE Accounts 
                            SET Balance = @Balance
                            WHERE Id = @AccountId";

            ExecuteWithTransactionHistory(deposit, query, acct.AccountNumber, newBalance, transactionInfo);

            return true;
        }

        public bool Withdraw(Withdrawal withdrawal, string transactionInfo = null)
        {
            var acct = _accountService.Get(withdrawal.AccountId);
            var newBalance = acct.Balance - withdrawal.Amount;

            var query = @"UPDATE Accounts 
                            SET Balance = @Balance
                            WHERE Id = @AccountId";

            ExecuteWithTransactionHistory(withdrawal, query, acct.AccountNumber, newBalance, transactionInfo);

            return true;
        }

        public bool Transfer(TransferFunds transfer)
        {
            var transferToAcct = _accountService.GetBy(transfer.TransferToAccountNumber);
            var acct = _accountService.Get(transfer.AccountId);

            if (transferToAcct != null)
            {
                Withdraw(new Withdrawal { AccountId = transfer.AccountId, Amount = transfer.Amount, LastTransactionDate = transfer.LastTransactionDate, TransactionType = TransactionTypes.Transfer, }, $"Fund transfer to Account Name: {transferToAcct.AccountName} Account Number: {transfer.TransferToAccountNumber}");
                Deposit(new Domain.Deposit { AccountId = transferToAcct.Id, Amount = transfer.Amount, LastTransactionDate = transfer.LastTransactionDate, TransactionType = TransactionTypes.Transfer }, $"Fund transfer from Account Name: {acct.AccountName}");
            }
            //DO Validations and return errors;

            return true;
        }

        public IEnumerable<TransactionHistory> GetHistory(int accountId)
        {
            return _database.QueryList<TransactionHistory>("SELECT * FROM TransactionHistory WHERE AccountId = @AccountId", new { AccountId = accountId });
        }

        private int ExecuteWithTransactionHistory(ITransaction transaction, string sqltransaction, string accountNumber, decimal newBalance, string transactionInfo = null)
        {
            var transactionTypeString = transaction.TransactionType.ToString();
            var transHistory = $"INSERT INTO TransactionHistory (AccountId, AccountNumber, TransactionType, TransactionInfo, TransactionAmount, CurrentBalance) " +
                                    $"VALUES (@AccountId, @AccountNumber, @TransactionType, @TransactionInfo, @TransactionAmount, @Balance)";

            var query = new StringBuilder();
            query.Append(sqltransaction).AppendLine();
            query.Append(transHistory);

            return _database.Execute(query.ToString(), new { AccountId = transaction.AccountId, AccountNumber = accountNumber, TransactionType = transactionTypeString, TransactionInfo = transactionInfo, TransactionAmount = transaction.Amount, Balance = newBalance });
        }
    }
}
