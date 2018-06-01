using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Domain;
using Bank.Web.Models;
using Bank.Web.ServiceClient;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Web.Controllers
{
    public class AccountController : Controller
    {
        private IHttpClient _httpClient;
        public AccountController(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View(new AccountViewModel());
        }

        [HttpPost]
        public IActionResult Resgister(AccountViewModel account)
        {
            _httpClient.PostAsync<int>("api/Account",
                    new Account { AccountName = account.AccountName,
                        AccountNumber = account.AccountNumber,
                        Password = account.Password,
                        Balance = account.Balance
                    });
            return View("Index");
        }
    }
}