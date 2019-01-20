using Queenwood.Core.Client.Ebay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Queenwood.Core.Client.Ebay
{
    public interface IEbayClient
    {
        dynamic SearchEbay(string searchTerm);
        Task<HttpResponseMessage> GetUserListings(string userID);
        GetSellerListResponse ProcessListings(Task<HttpResponseMessage> message);
    }
}
