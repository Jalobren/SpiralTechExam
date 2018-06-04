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
    }
}
