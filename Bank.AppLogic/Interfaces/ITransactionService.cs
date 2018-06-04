using Bank.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.AppLogic.Interfaces
{
    public interface ITransactionService
    {
        TransactionResponse Deposit(Deposit deposit, string transactionInfo = null);
        TransactionResponse Withdraw(Withdrawal withdrawal, string transactionInfo = null);
        TransactionResponse Transfer(TransferFunds transfer);
        IEnumerable<TransactionHistory> GetHistory(int accountId);
    }
}
