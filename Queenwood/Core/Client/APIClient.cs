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
        private readonly HttpClient _client;

        protected APIClient(IBaseClient baseClient)
        {
            _client = baseClient.GetHttpClient();
        }

        protected async Task<string> GetStringAsync(string baseUrl, string url)
        {
            return await _client.GetStringAsync(baseUrl + url);
        }

        protected async Task<APIResult<T>> GetAsync<T>(string baseUrl, string url)
        {
            HttpResponseMessage response = await _client.GetAsync(baseUrl + url);

            return await CreateAPIResultAsync<T>(response);
        }

        protected async Task<APIResult> PostAsync<T>(string baseUrl, string url, T payload)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync<T>(url, payload);

            return await CreateAPIResultAsync(response);
        }

        protected async Task<APIResult> PostFormUrlAsync(string baseUrl, string url, Dictionary<string, string> payload)
        {
            var content = new FormUrlEncodedContent(payload);

            HttpResponseMessage response = await _client.PostAsync(baseUrl + url, content);

            return await CreateAPIResultAsync(response);
        }

        private async Task<APIResult> CreateAPIResultAsync(HttpResponseMessage response)
        {
            var result = new APIResult
            {
                Json = await response.Content.ReadAsStringAsync()
            };

            return result;
        }

        private async Task<APIResult<T>> CreateAPIResultAsync<T>(HttpResponseMessage response)
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
    }
}
