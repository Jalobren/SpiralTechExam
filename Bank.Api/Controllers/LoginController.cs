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
    [Route("api/Login")]
    public class LoginController : Controller
    {
        private IAccountService _accountService;
        public LoginController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public Account UserLogin([FromBody]Login loginDomain)
        {
            var account = _accountService.GetBy(loginDomain.AccountNumber, loginDomain.Password);
            return account;
        }
    }
}