using Bank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Web.Models.Transactions
{
    public class WithdrawalViewModel : Withdrawal
    {
        public string AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
