using Bank.Domain;
using Bank.Web.Models.Account;
using Bank.Web.ServiceClient;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            return View(new AccountModel());
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _httpClient.GetAsync<Account>($"api/Account/{id}");
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveRegistration(AccountModel accountModel)
        {
            var result = await _httpClient.PostAsync<int>("api/Account",
                    new Account { AccountName = accountModel.AccountName,
                        AccountNumber = accountModel.AccountNumber,
                        Password = accountModel.Password,
                        Balance = accountModel.Balance
                    });
            return RedirectToAction("Detail", new { id = result });
        }
    }
}