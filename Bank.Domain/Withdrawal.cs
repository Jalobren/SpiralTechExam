using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Domain
{
    public class Withdrawal
    {
        public int AccountId { get; set; }
        public decimal WithdrawalAmount { get; set; }
        public DateTime LastTransactionDate { get; set; }
    }
}
