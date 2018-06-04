using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Domain.Interfaces
{
    public interface ITransaction
    {
        int AccountId { get; set; }
        Decimal Amount { get; set; }
        TransactionTypes TransactionType { get; set; }
    }
}
