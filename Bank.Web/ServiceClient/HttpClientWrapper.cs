using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Web.ServiceClient
{
    public class HttpClientWrapper : IHttpClient
    {
        private System.Net.Http.HttpClient _httpClient;

        private string _baseUrl { get; set; }

        public HttpClientWrapper(IConfiguration configuration)
        {
            _baseUrl = configuration["Api:BaseUrl"];
        }

        public async Task<T> GetAsync<T>(string relativeUrl)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _baseUrl + relativeUrl);
            var apiResponse = await this.Client.SendAsync(request);

            if (apiResponse.IsSuccessStatusCode)
            {
                var responseData = await apiResponse.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(responseData))
                {
                    return JsonConvert.DeserializeObject<T>(responseData);
                }
            }
            else
            {
                return default(T);
            }
            return default(T);
        }

        
        public async Task<T> PostAsync<T>(string relativeUrl, object body)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl + relativeUrl);

            var content = JsonConvert.SerializeObject(body);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await this.Client.SendAsync(request);
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());

        }

        protected HttpClient Client
        {
            get
            {
                if (this._httpClient == null)
                {
                    this._httpClient = new HttpClient();
                }

                return this._httpClient;
            }
        }
    }
}
