using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Web.Models.Account
{
    public class AccountModel
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
    }
}
