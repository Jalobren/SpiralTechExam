using Bank.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Domain
{
    public class TransferFunds : ITransaction
    {
        public TransferFunds()
        {
            TransactionType = TransactionTypes.Transfer;
        }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string TransferToAccountNumber { get; set; }
        public DateTime LastTransactionDate { get; set; }
        public TransactionTypes TransactionType { get; set; }
    }
}
