using Bank.AppLogic.Interfaces;
using Bank.Domain;
using System;
using System.Collections.Generic;

namespace Bank.AppLogic
{
    public class AccountService : IAccountService
    {
        private IDatabase _database;
        public AccountService(IDatabase database)
        {
            _database = database;
        }
        public Account Get(int id)
        {
            return _database.Query<Account>("SELECT * FROM Accounts WHERE Id = @Id", new { Id = id });
        }

        public Account GetBy(string accountNumber)
        {
            return _database.Query<Account>("SELECT * FROM Accounts WHERE AccountNumber = @AccountNumber", new { AccountNumber = accountNumber });
        }

        public Account GetBy(string accountNumber, string password)
        {
            return _database.Query<Account>("SELECT * FROM Accounts WHERE AccountNumber = @AccountNumber AND Password = @Password", new { AccountNumber = accountNumber, Password = password });
        }

        public IEnumerable<Account> Get()
        {
            return _database.QueryList<Account>("SELECT * FROM Accounts");
        }

        public TransactionResponse Create(Account account)
        {
            var response = ValidateAccount(account);

            if (response.IsSuccess)
            {
                var query = @"INSERT INTO Accounts 
                            (AccountNumber,AccountName,Password, Balance) 
                        OUTPUT INSERTED.[Id]
                        values 
                            (@AccountNumber,@AccountName,@Password, @Balance)";

                _database.Query<int>(query, account);
            }
            return response;
        }

        public TransactionResponse ValidateAccount(Account account)
        {
            var response = ValidateDuplicateAccountNumber(account);
            if (response.IsSuccess)
            {
                return ValidateDuplicateAccountName(account);
            }
            return response;
        }

        public TransactionResponse ValidateDuplicateAccountNumber(Account account)
        {
            var accnt = _database.Query<Account>("SELECT * FROM Accounts WHERE AccountNumber = @AccountNumber", new { AccountNumber = account.AccountNumber });

            if (accnt != null)
            {
                return new TransactionResponse { ErrorCode = ErrorCodes.ERR_ACCOUNT_DUPLICATE_ACCT_NUMBER };
            }
            return new TransactionResponse { IsSuccess = true };
        }
        public TransactionResponse ValidateDuplicateAccountName(Account account)
        {
            var accnt = _database.Query<Account>("SELECT * FROM Accounts WHERE AccountName = @AccountName", new { AccountName = account.AccountName });

            if (accnt != null)
            {
                return new TransactionResponse { ErrorCode = ErrorCodes.ERR_ACCOUNT_DUPLICATE_ACCT_NAME };
            }
            return new TransactionResponse { IsSuccess = true };
        }
    }
}
