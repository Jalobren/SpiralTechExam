using Bank.AppLogic.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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

        public int Execute(string sql, object parameters = null, IDbTransaction transaction = null)
        {
            using (var connection = Connection)
            {
                return connection.Execute(sql, parameters, transaction);
            }
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
    }
}
