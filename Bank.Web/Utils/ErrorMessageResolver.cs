using Bank.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Web.Utils
{
    public static class ErrorMessageResolver
    {
        public static string GetErrorMessage(string errorCode)
        {
            string message;
            switch (errorCode)
            {
                case ErrorCodes.ERR_CONCURRENT:
                    message = "Recent transactions has been made, your current balance is updated. Please try again.";
                    break;
                default:
                    message = "Unknown error occured.";
                    break;
            }
            return message;
        }
    }
}
