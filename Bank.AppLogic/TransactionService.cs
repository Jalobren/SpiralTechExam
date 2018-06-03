using Bank.AppLogic.Interfaces;
using Bank.Domain;
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

        public bool Deposit(Deposit deposit)
        {
            var acct = _accountService.Get(deposit.AccountId);
            acct.Balance = acct.Balance + deposit.DepositAmount;

            var query = @"UPDATE Accounts 
                            SET Balance = @Balance
                            WHERE Id = @Id";
            _database.Query<int>(query, acct);

            return true;
        }

        public bool Withdraw(Withdrawal withdrawal)
        {
            var acct = _accountService.Get(withdrawal.AccountId);
            acct.Balance = acct.Balance - withdrawal.WithdrawalAmount;

            var query = @"UPDATE Accounts 
                            SET Balance = @Balance
                            WHERE Id = @Id";
            _database.Query<int>(query, acct);

            return true;
        }
    }
}
