using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.AppLogic.Interfaces;
using Bank.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Transaction")]
    public class TransactionController : Controller
    {
        private ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        [Route("Deposit")]
        public bool DepositTransaction([FromBody]Deposit deposit)
        {
            return _transactionService.Deposit(deposit);
        }

        [HttpPost]
        [Route("Withdraw")]
        public bool WithdrawTransaction([FromBody]Withdrawal withdrawal)
        {
            return _transactionService.Withdraw(withdrawal);
        }

        [HttpPost]
        [Route("Transfer")]
        public bool TransferFundsTransaction([FromBody]TransferFunds transferFunds)
        {
            return _transactionService.Transfer(transferFunds);
        }

        [HttpGet]
        [Route("History/{id}")]
        public IEnumerable<TransactionHistory> GetTransactionHistory(int id)
        {
            return _transactionService.GetHistory(id);
        }
    }
}