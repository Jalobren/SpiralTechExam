using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Domain;
using Bank.Web.Models.Transactions;
using Bank.Web.ServiceClient;
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
        public async Task<IActionResult> Deposit(int id)
        {
            var result = await _httpClient.GetAsync<Account>($"api/Account/{id}");

            return View(new DepositViewModel { AccountId = result.Id, AccountNumber = result.AccountNumber, CurrentBalance = result.Balance, LastTransactionDate = result.LastTransactionDate });
        }

        [HttpPost]
        public async Task<IActionResult> DepositTransaction(DepositViewModel depositModel)
        {
            await _httpClient.PostAsync<bool>("api/Transaction/Deposit",
                 new Deposit
                 {
                     AccountId = depositModel.AccountId,
                     DepositAmount = depositModel.DepositAmount,
                     LastTransactionDate = depositModel.LastTransactionDate
                 });
            return RedirectToAction("Detail","Account", new { id = depositModel.AccountId });
        }

        [HttpGet]
        public async Task<IActionResult> Withdraw(int id)
        {
            var result = await _httpClient.GetAsync<Account>($"api/Account/{id}");

            return View(new WithdrawalViewModel { AccountId = result.Id, AccountNumber = result.AccountNumber, CurrentBalance = result.Balance, LastTransactionDate = result.LastTransactionDate });
        }

        [HttpPost]
        public async Task<IActionResult> WithdrawTransaction(WithdrawalViewModel withdrawModel)
        {
            await _httpClient.PostAsync<bool>("api/Transaction/Withdraw",
                 new WithdrawalViewModel
                 {
                     AccountId = withdrawModel.AccountId,
                     WithdrawalAmount = withdrawModel.WithdrawalAmount,
                     LastTransactionDate = withdrawModel.LastTransactionDate
                 });
            return RedirectToAction("Detail", "Account", new { id = withdrawModel.AccountId });
        }
    }
}