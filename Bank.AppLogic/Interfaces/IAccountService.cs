using Bank.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.AppLogic.Interfaces
{
    public interface IAccountService
    {
        Account Get(int id);
        IEnumerable<Account> Get();
        int Create(Account account);
    }
}
