using Microsoft.Extensions.Options;
using Queenwood.Models.Config;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
using Queenwood.Core.Client.Etsy.Model;
using System.Linq;
using Queenwood.Core.Client.Etsy.Consts;
using System.Collections.Generic;
using Queenwood.Core.Services.CacheService;

namespace Queenwood.Core.Client.Etsy
{
    public class EtsyClient : APIClient, IEtsyClient
    {
        private readonly EtsyConfig _etsyConfig;
        private readonly ICacheService _cacheService;

        public EtsyClient(IOptions<EtsyConfig> etsyConfig, ICacheService cacheService, IBaseClient baseClient) : base(baseClient)
        {
            _etsyConfig = etsyConfig.Value;
            _cacheService = cacheService;
        }

        public async Task<List<EtsyListing>> GetListings()
        {
            return await _cacheService.GetAsync("GetEtsyListings", async () =>
            {
                NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
                queryString.Add("api_key", _etsyConfig.EtsyToken);
                queryString.Add("includes", "Listings/Images");

                var result = await GetAsync<EtsyAPICall>(_etsyConfig.BaseUrl, $"/v2/shops/{_etsyConfig.EtsyShopId}?{queryString}");

                var shop = result.Data.results.First();

                return shop.Listings.Where(x => x.state == ListingState.Active) // for sold out listings: ListingState.SoldOut
                             .Where(x => x.Images != null && x.Images.Any()) // with images
                             .ToList();

            }, 1440).Unwrap();
        }
    }
}
