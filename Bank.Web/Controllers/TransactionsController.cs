using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Domain;
using Bank.Web.Models.Transactions;
using Bank.Web.ServiceClient;
using Bank.Web.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Web.Controllers
{
    public class TransactionsController : Controller
    {
        private IHttpClient _httpClient;
        public TransactionsController(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Deposit(int id, string err = null)
        {
            var result = await _httpClient.GetAsync<Account>($"api/Account/{id}");

            return View(new DepositViewModel { AccountId = result.Id, AccountNumber = result.AccountNumber, CurrentBalance = result.Balance, LastTransactionDate = result.LastTransactionDate, ErrorMessage = err });
        }

        [HttpPost]
        public async Task<IActionResult> DepositTransaction(DepositViewModel depositModel)
        {
            var response = await _httpClient.PostAsync<TransactionResponse>("api/Transaction/Deposit", depositModel);
            if (!response.IsSuccess)
            {
                var errorMessage = ErrorMessageResolver.GetErrorMessage(response.ErrorCode);
                return RedirectToAction("Deposit", new { id = depositModel.AccountId, err = errorMessage });
            }
            return RedirectToAction("Detail","Account", new { id = depositModel.AccountId });
        }

        [HttpGet]
        public async Task<IActionResult> Withdraw(int id, string err = null)
        {
            var result = await _httpClient.GetAsync<Account>($"api/Account/{id}");

            return View(new WithdrawalViewModel { AccountId = result.Id, AccountNumber = result.AccountNumber, CurrentBalance = result.Balance, LastTransactionDate = result.LastTransactionDate, ErrorMessage = err });
        }

        [HttpPost]
        public async Task<IActionResult> WithdrawTransaction(WithdrawalViewModel withdrawModel)
        {
            var response = await _httpClient.PostAsync<TransactionResponse>("api/Transaction/Withdraw", withdrawModel);
            if (!response.IsSuccess)
            {
                var errorMessage = ErrorMessageResolver.GetErrorMessage(response.ErrorCode);
                return RedirectToAction("Withdraw", new { id = withdrawModel.AccountId, err = errorMessage });
            }
            return RedirectToAction("Detail", "Account", new { id = withdrawModel.AccountId });
        }

        [HttpGet]
        public async Task<IActionResult> Transfer(int id, string err)
        {
            var result = await _httpClient.GetAsync<Account>($"api/Account/{id}");

            return View(new TransferFundsViewModel { AccountId = result.Id, AccountNumber = result.AccountNumber, CurrentBalance = result.Balance, LastTransactionDate = result.LastTransactionDate, ErrorMessage = err });
        }

        [HttpPost]
        public async Task<IActionResult> TransferTransaction(TransferFundsViewModel transferFundsModel)
        {
            var response = await _httpClient.PostAsync<TransactionResponse>("api/Transaction/Transfer", transferFundsModel);
            if (!response.IsSuccess)
            {
                var errorMessage = ErrorMessageResolver.GetErrorMessage(response.ErrorCode);
                return RedirectToAction("Transfer", new { id = transferFundsModel.AccountId, err = errorMessage });
            }
            return RedirectToAction("Detail", "Account", new { id = transferFundsModel.AccountId });
        }
        [HttpGet]
        public async Task<IActionResult> History(int id)
        {
            var result = await _httpClient.GetAsync<IEnumerable<TransactionHistory>>($"api/Transaction/History/{id}");

            return View(result);
        }
    }
}