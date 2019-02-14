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
        Task<dynamic> SearchEbay(string searchTerm);
        Task<List<Item>> GetUserListings(string userID);
    }
}
