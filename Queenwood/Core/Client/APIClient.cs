using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Queenwood.Core.Client
{
    public abstract class APIClient
    {
        private HttpClient _client;

        protected APIClient(IBaseClient baseClient)
        {
            _client = baseClient.GetHttpClient();
        }

        protected async Task<string> GetString(string baseUrl, string url)
        {
            //SetupHttpClient(baseUrl);

            return await _client.GetStringAsync(baseUrl + url);
        }

        protected async Task<APIResult<T>> Get<T>(string baseUrl, string url)
        {
            //SetupHttpClient(baseUrl);

            HttpResponseMessage response = await _client.GetAsync(baseUrl + url);

            return await CreateAPIResult<T>(response);
        }

        //protected async Task<APIResult> Post<T>(string baseUrl, string url, T payload)
        //{
        //    SetupHttpClient(baseUrl);

        //    HttpResponseMessage response = await _client.PostAsJsonAsync<T>(url, payload);

        //    return await CreateAPIResult(response);
        //}

        protected async Task<APIResult> PostFormUrl(string baseUrl, string url, Dictionary<string, string> payload)
        {
            //SetupHttpClient(baseUrl);

            var content = new FormUrlEncodedContent(payload);

            HttpResponseMessage response = await _client.PostAsync(baseUrl + url, content);

            return await CreateAPIResult(response);
        }

        private async Task<APIResult> CreateAPIResult(HttpResponseMessage response)
        {
            var result = new APIResult
            {
                Json = await response.Content.ReadAsStringAsync()
            };

            return result;
        }

        private async Task<APIResult<T>> CreateAPIResult<T>(HttpResponseMessage response)
        {
            var result = new APIResult<T>();

            if (response.IsSuccessStatusCode)
            {
                var getData = response.Content.ReadAsAsync<T>();
                var getJson = response.Content.ReadAsStringAsync();

                await Task.WhenAll(getData, getJson);

                result.Data = getData.Result;
                result.Json = getJson.Result;
            }
            else
            {
                result.Json = await response.Content.ReadAsStringAsync();
            }

            return result;
        }

        //private void SetupHttpClient(string baseUrl)
        //{
        //    _client.BaseAddress = new Uri(baseUrl);

        //    //var c = new HttpClient
        //    //{
        //    //    BaseAddress = new Uri(baseUrl)
        //    //};

        //    //c.DefaultRequestHeaders.Accept.Clear();
        //    //c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            
        //}
    }
}
