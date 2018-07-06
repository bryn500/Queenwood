using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Queenwood.Core.Client
{
    public abstract class APIClient
    {
        protected async Task<APIResult<T>> Get<T>(string baseUrl, string url)
        {
            using (var httpClient = GetHttpClient(baseUrl))
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);

                return await CreateAPIResult<T>(response);
            }
        }

        protected async Task<APIResult> Post<T>(string baseUrl, string url, T payload)
        {
            using (var httpClient = GetHttpClient(baseUrl))
            {
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<T>(url, payload);

                return await CreateAPIResult(response);
            }
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

        private HttpClient GetHttpClient(string baseUrl)
        {
            var c = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            c.DefaultRequestHeaders.Accept.Clear();
            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return c;
        }
    }
}
