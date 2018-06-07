using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Domain
{
    public class QueryResponse<T>
    {
        public T Result { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorCode { get; set; }
    }
}
