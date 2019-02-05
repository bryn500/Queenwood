using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Queenwood.Core.Client
{
    public interface IBaseClient
    {
        HttpClient GetHttpClient();
    }
}
