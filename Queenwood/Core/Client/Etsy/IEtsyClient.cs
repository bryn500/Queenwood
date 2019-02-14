using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Queenwood.Core.Client.Etsy.Model;

namespace Queenwood.Core.Client.Etsy
{
    public interface IEtsyClient
    {
        Task<List<EtsyListing>> GetListings();
    }
}
