using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Web.ServiceClient
{
    public interface IHttpClient
    {
        Task<T> GetAsync<T>(string relativeUrl);
        Task<T> PostAsync<T>(string relativeUrl, object body);
    }
}
