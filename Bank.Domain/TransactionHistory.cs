using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Domain
{
    public class TransactionHistory
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public TransactionTypes TransactionType { get; set; }
        public string TransactionInfo { get; set; }
        public decimal TransactionAmount { get; set; }
        public decimal CurrentBalance { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public enum TransactionTypes
    {
        Deposit = 0,
        Withdrawal = 1,
        Transfer = 2
    }
}
