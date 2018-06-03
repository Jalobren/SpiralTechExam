using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Domain
{
    public class Deposit
    {
        public int AccountId { get; set; }
        public decimal DepositAmount { get; set; }
        public DateTime LastTransactionDate { get; set; }
    }
}
