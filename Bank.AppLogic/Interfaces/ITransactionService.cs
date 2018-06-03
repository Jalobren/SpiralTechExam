using Bank.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.AppLogic.Interfaces
{
    public interface ITransactionService
    {
        bool Deposit(Deposit deposit);
        bool Withdraw(Withdrawal withdrawal);
    }
}
