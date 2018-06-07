using Bank.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;

namespace Bank.AppLogic.Interfaces
{
    public interface IDatabase
    {
        T Query<T>(string sql, object parameters = null);
        IEnumerable<T> QueryList<T>(string sql, object parameters = null);
        TransactionResponse Execute(string sql, object parameters = null);
        TransactionScope GetTransaction(int? timeoutInSec = null, TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required,
                                            System.Transactions.IsolationLevel isolationLevel = System.Transactions.IsolationLevel.ReadCommitted);
    }
}
