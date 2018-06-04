using Bank.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Domain
{
    public class Withdrawal : ITransaction
    {
        public Withdrawal()
        {
            TransactionType = TransactionTypes.Withdrawal;
        }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime LastTransactionDate { get; set; }
        public TransactionTypes TransactionType { get; set; }
    }
}
