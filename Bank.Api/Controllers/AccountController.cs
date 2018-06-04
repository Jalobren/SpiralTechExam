using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bank.AppLogic.Interfaces;
using Bank.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Route("{id}")]
        public Account Get(int id)
        {
            return _accountService.Get(id);
        }

        [HttpGet]
        public IEnumerable<Account> Get()
        {
            return _accountService.Get();
        }

        [HttpPost]
        [Route("access")]
        public Account AccessAccount([FromBody]AccountAccess accountAccessDomain)
        {
            var account = _accountService.GetBy(accountAccessDomain.AccountNumber, accountAccessDomain.Password);
            return account;
        }

        [HttpPost]
        public int Create([FromBody]Account account)
        {
            return _accountService.Create(account);
        }
    }
}