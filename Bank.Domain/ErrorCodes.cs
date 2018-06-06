using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Domain
{
    public static class ErrorCodes
    {
        public const string ERR_CONCURRENT = "ERR_CONCURRENT_REQUEST";
        public const string ERR_ACCOUNT_NOT_FOUND = "ERR_ACCOUNT_NOT_FOUND";
        public const string ERR_INSUFFICIENT_FUNDS = "ERR_INSUFFICIENT_FUNDS";
        public const string ERR_ACCOUNT_DUPLICATE_ACCT_NUMBER = "ERR_ACCOUNT_DUPLICATE_ACCT_NUMBER";
        public const string ERR_ACCOUNT_DUPLICATE_ACCT_NAME = "ERR_ACCOUNT_DUPLICATE_ACCT_NAME";
    }
}
