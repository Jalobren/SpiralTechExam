using Bank.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.AppLogic.Interfaces
{
    public interface ITransactionService
    {
        bool Deposit(Deposit deposit, string transactionInfo = null);
        bool Withdraw(Withdrawal withdrawal, string transactionInfo = null);
        bool Transfer(TransferFunds transfer);
        IEnumerable<TransactionHistory> GetHistory(int accountId);
    }
}
