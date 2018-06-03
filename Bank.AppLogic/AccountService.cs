﻿using Bank.AppLogic.Interfaces;
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

        public Account GetBy(string accountNumber, string password)
        {
            return _database.Query<Account>("SELECT * FROM Accounts WHERE AccountNumber = @AccountNumber AND Password = @Password", new { AccountNumber = accountNumber, Password = password });
        }

        public IEnumerable<Account> Get()
        {
            return _database.QueryList<Account>("SELECT * FROM Accounts");
        }

        public int Create(Account account)
        {
            var query = @"INSERT INTO Accounts 
                            (AccountNumber,AccountName,Password, Balance) 
                        OUTPUT INSERTED.[Id]
                        values 
                            (@AccountNumber,@AccountName,@Password, @Balance)";
            return _database.Query<int>(query, account);
        }
    }
}