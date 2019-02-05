using Microsoft.Extensions.Options;
using Queenwood.Models.Config;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
using Queenwood.Core.Client.Etsy.Model;
using System.Linq;
using Queenwood.Core.Client.Etsy.Consts;
using System.Collections.Generic;

namespace Queenwood.Core.Client.Etsy
{
    public class EtsyClient : APIClient, IEtsyClient
    {
        private readonly EtsyConfig _etsyConfig;

        public EtsyClient(IOptions<EtsyConfig> etsyConfig, IBaseClient baseClient) : base(baseClient)
        {
            _etsyConfig = etsyConfig.Value;
        }

        public async Task<APIResult<EtsyAPICall>> GetListings()
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add("api_key", _etsyConfig.EtsyToken);
            queryString.Add("includes", "Listings/Images");

            return await Get<EtsyAPICall>(_etsyConfig.BaseUrl, $"/v2/shops/{_etsyConfig.EtsyShopId}?{queryString}");
        }

        public List<EtsyListing> ProcessListings(Task<APIResult<EtsyAPICall>> apiResult)
        {
            var shop = apiResult.Result.Data.results.First();

            return shop.Listings.Where(x => x.state == ListingState.Active) // for sold out listings: ListingState.SoldOut
                         .Where(x => x.Images != null && x.Images.Any()) // with images
                         .ToList();
        }
    }
}
