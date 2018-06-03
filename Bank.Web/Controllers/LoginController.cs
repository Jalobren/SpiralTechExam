using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Domain;
using Bank.Web.Models.Account;
using Bank.Web.Models.Login;
using Bank.Web.ServiceClient;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Web.Controllers
{
    public class LoginController : Controller
    {
        private IHttpClient _httpClient;
        public LoginController(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            var acct = await _httpClient.PostAsync<Account>("api/Login", new Login { AccountNumber = loginModel.AccountNumber, Password = loginModel.Password });

            return RedirectToAction("Detail","Account", new { id = acct.Id });
        }
    }
}