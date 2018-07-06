using Microsoft.Extensions.Options;
using Queenwood.Models.Config;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
using Queenwood.Core.Client.Etsy.Model;

namespace Queenwood.Core.Client.Etsy
{
    public class EtsyClient : APIClient, IEtsyClient
    {
        private readonly EtsyConfig _etsyConfig;

        public EtsyClient(IOptions<EtsyConfig> etsyConfig)
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
    }
}
