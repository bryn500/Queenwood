using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Queenwood.Core.Client.Ebay.Model;
using Queenwood.Core.Services.CacheService;
using Queenwood.Core.Services.ContentfulService;
using Queenwood.Models.Config;

namespace Queenwood.Core.Client.Ebay
{
    public class EbayClient : APIClient, IEbayClient
    {
        private readonly EbayConfig _ebayConfig;
        private readonly IContentfulService _contentfulService;
        private readonly ICacheService _cacheService;
        private readonly IBaseClient _baseClient;

        public EbayClient(IOptions<EbayConfig> ebayConfig, IContentfulService contentfulService, ICacheService cacheService, IBaseClient baseClient) : base(baseClient)
        {
            _ebayConfig = ebayConfig.Value;
            _contentfulService = contentfulService;
            _cacheService = cacheService;
            _baseClient = baseClient;
        }

        // Finding API
        public async Task<dynamic> SearchEbay(string searchTerm)
        {
            const int pageSize = 30;

            StringBuilder sb = new StringBuilder();

            // API
            sb.Append("services/search/FindingService/v1");

            // Endpoint
            sb.Append("?OPERATION-NAME=findItemsByKeywords");

            // Metadata
            sb.Append("&SERVICE-VERSION=1.0.0&RESPONSE-DATA-FORMAT=JSON");

            // Auth
            sb.AppendFormat("&SECURITY-APPNAME={0}", _ebayConfig.ClientId);

            // Payload
            sb.AppendFormat("&REST-PAYLOAD&keywords={0}&paginationInput.entriesPerPage={1}&GLOBAL-ID=EBAY-GB&siteid=3", HttpUtility.UrlEncode(searchTerm), pageSize);

            var json = await GetStringAsync("https://svcs.ebay.com/", sb.ToString());

            dynamic data = JsonConvert.DeserializeObject(json);

            return data;
        }

        // Trading API
        public async Task<List<Item>> GetUserListings(string userID)
        {
            return await _cacheService.GetAsync("GetEbayUserListings", async () =>
            {
                var client = _baseClient.GetHttpClient();

                var task = await client.SendAsync(BuildGetUserListingsRequest(userID));
                var responseContent = await task.Content.ReadAsStringAsync();

                var results = DeserializeResult(responseContent);

                return await ProcessResults(results);
            }, 1440).Unwrap();
        }

        private GetSellerListResponse DeserializeResult(string responseContent)
        {
            var result = new GetSellerListResponse();

            var serializer = new XmlSerializer(typeof(GetSellerListResponse));

            using (var reader = new StringReader(responseContent))
            {
                result = (GetSellerListResponse)serializer.Deserialize(reader);
            }

            return result;
        }

        private async Task<List<Item>> ProcessResults(GetSellerListResponse result)
        {
            // Filter categories from contentful
            var categoryFilters = await _contentfulService.GetEbayCategoryFilters();
            var includeCategories = categoryFilters.Where(x => x.IncludeOrIgnore).Select(x => x.Category).ToList();
            var excludeCategoris = categoryFilters.Where(x => !x.IncludeOrIgnore).Select(x => x.Category).ToList();

            foreach (var item in result.ItemArray.Items)
            {
                // Hide all items by default
                item.Hide = true;

                // Get list of cagtegories
                var categories = item.PrimaryCategory.CategoryName.Split(':').ToList();

                // Show items that are in an include category
                foreach (var include in includeCategories)
                    if (categories.Contains(include))
                        item.Hide = false;

                // Hide any that are also in an excluded category
                foreach (var exclude in excludeCategoris)
                    if (categories.Contains(exclude))
                        item.Hide = true;
            }

            result.ItemArray.Items.RemoveAll(x => x.Hide);

            return result.ItemArray.Items;
        }

        private HttpRequestMessage BuildGetUserListingsRequest(string userID)
        {
            const string methodName = "GetSellerList";
            const string version = "1085";
            const int siteId = 3;
            const int pageSize = 12;

            var today = DateTime.Now;
            var maxFuture = today.AddDays(100);
            var maxPast = today.AddDays(-100);

            XNamespace ns = "urn:ebay:apis:eBLBaseComponents";

            var xml = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement(ns + methodName + "Request",
                    new XElement(ns + "Version", version),
                    new XElement(ns + "UserID", userID),
                    new XElement(ns + "GranularityLevel", "Coarse"),
                    new XElement(ns + "EndTimeFrom", today.ToString("s", System.Globalization.CultureInfo.InvariantCulture)),
                    new XElement(ns + "EndTimeTo", maxFuture.ToString("s", System.Globalization.CultureInfo.InvariantCulture)),
                    new XElement(ns + "RequesterCredentials",
                        new XElement(ns + "eBayAuthToken", _ebayConfig.AuthnAuthToken)),
                    new XElement(ns + "Pagination",
                        new XElement(ns + "EntriesPerPage", pageSize),
                        new XElement(ns + "PageNumber", "1"))
            ));

            string payload;

            using (var sw = new MemoryStream())
            {
                using (var strw = new StreamWriter(sw, Encoding.UTF8))
                {
                    xml.Save(strw);
                    payload = Encoding.UTF8.GetString(sw.ToArray());
                }
            }

            var httpContent = new StringContent(payload, Encoding.UTF8, "application/xml");

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri("https://api.ebay.com/ws/api.dll"),
                Method = HttpMethod.Post,
                Content = httpContent
            };

            request.Headers.Add("X-EBAY-API-COMPATIBILITY-LEVEL", version);
            request.Headers.Add("X-EBAY-API-CALL-NAME", methodName);
            request.Headers.Add("X-EBAY-API-SITEID", siteId.ToString());

            return request;
        }
    }
}
