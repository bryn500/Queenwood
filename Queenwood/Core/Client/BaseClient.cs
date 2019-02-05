using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Queenwood.Core.Client
{
    public class BaseClient : IBaseClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        // HttpClient implements IDisposable but is generally not supposed to be disposed of for the lifetime of your application.
        // This is because whenever you make a request with the HttpClient and immediately dispose of it, you leave the connection open in a TIME_WAIT state.
        // It will remain in this state for 240 seconds by default. If you make a lot of requests in a short period you might end up exhausting the connection pool, which would result in a SocketException. 
        // To avoid this you should share a single instance of HttpClient for the entire application
        public HttpClient GetHttpClient()
        {
            _httpClient.DefaultRequestHeaders.ConnectionClose = false;
            return _httpClient;
        }
    }
}
