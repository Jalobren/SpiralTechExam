using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Bank.AppLogic.Interfaces
{
    public interface IDatabase
    {
        T Query<T>(string sql, object parameters = null);
        IEnumerable<T> QueryList<T>(string sql, object parameters = null);
        int Execute(string sql, object parameters = null, IDbTransaction transaction = null);
    }
}
