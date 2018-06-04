using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bank.Web.Models;
using Bank.Web.ServiceClient;
using Bank.Domain;

namespace Bank.Web.Controllers
{
    public class HomeController : Controller
    {
        private IHttpClient _httpClient;
        public HomeController(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _httpClient.GetAsync<IEnumerable<Account>>($"api/Account/");

            return View(result);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
