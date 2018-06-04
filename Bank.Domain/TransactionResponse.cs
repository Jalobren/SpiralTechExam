using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Domain
{
    public class TransactionResponse
    {
        public bool IsSuccess { get; set; }
        public string ErrorCode { get; set; }
    }

    
}
