using Bank.AppLogic.Interfaces;
using Bank.Domain;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace Bank.Data
{
    public class Database : IDatabase
    {
        protected string _connectionString;
        public Database(IConfiguration configuration)
        {
            _connectionString = configuration["Database:ConnectionString"];
        }

        protected IDbConnection Connection
        {
            get { return new SqlConnection(_connectionString); }
        }

        public TransactionResponse Execute(string sql, object parameters = null)
        {
            var response = new TransactionResponse();
            try
            {
                using (var connection = Connection)
                {
                    connection.Execute(sql, parameters);
                    response.IsSuccess = true;
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 1205)
                {
                    response.ErrorCode = ErrorCodes.ERR_CONCURRENT;
                }
                else
                {
                    response.ErrorCode = ErrorCodes.ERR_UNKOWN;
                }
            }
            catch (Exception ex)
            {
                response.ErrorCode = ErrorCodes.ERR_UNKOWN;
            }

            return response;
        }
        public T Query<T>(string sql, object parameters = null)
        {
            return QueryList<T>(sql, parameters).FirstOrDefault();
        }

        public IEnumerable<T> QueryList<T>(string sql, object parameters = null)
        {
            using (var connection = Connection)
            {
                return connection.Query<T>(sql, parameters);
            }
        }

        public TransactionScope GetTransaction(int? timeoutInSec = null, TransactionScopeOption transactionScopeOption = TransactionScopeOption.Required,
                                            System.Transactions.IsolationLevel isolationLevel = System.Transactions.IsolationLevel.ReadCommitted)
        {
            return new TransactionScope(transactionScopeOption,
                new TransactionOptions() { IsolationLevel = isolationLevel, Timeout = (timeoutInSec == null) ? TransactionManager.DefaultTimeout : new TimeSpan(0, 0, timeoutInSec.Value) });
        }
    }
}
