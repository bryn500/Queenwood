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
using Queenwood.Models.Config;

namespace Queenwood.Core.Client.Ebay
{
    public class EbayClient : APIClient, IEbayClient
    {
        private readonly EbayConfig _ebayConfig;

        public EbayClient(IOptions<EbayConfig> ebayConfig)
        {
            _ebayConfig = ebayConfig.Value;
        }

        // Finding API
        public dynamic SearchEbay(string searchTerm)
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

            var json = GetString("https://svcs.ebay.com/", sb.ToString()).Result;

            dynamic data = JsonConvert.DeserializeObject(json);

            return data;
        }        

        // Trading API
        public async Task<HttpResponseMessage> GetUserListings(string userID)
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

            using (var httpClient = new HttpClient())
            {
                var httpContent = new StringContent(payload, Encoding.UTF8, "application/xml"); // or application/xml

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("https://api.ebay.com/ws/api.dll"),
                    Method = HttpMethod.Post,
                    Content = httpContent
                };

                request.Headers.Add("X-EBAY-API-COMPATIBILITY-LEVEL", version);
                request.Headers.Add("X-EBAY-API-CALL-NAME", methodName);
                request.Headers.Add("X-EBAY-API-SITEID", siteId.ToString());

                return await httpClient.SendAsync(request);
            }
        }

        public GetSellerListResponse ProcessListings(Task<HttpResponseMessage> message)
        {
            var responseContent = message.Result.Content.ReadAsStringAsync().Result;

            var serializer = new XmlSerializer(typeof(GetSellerListResponse));

            GetSellerListResponse result = new GetSellerListResponse();

            using (var reader = new StringReader(responseContent))
            {
                result = (GetSellerListResponse)serializer.Deserialize(reader);
            }

            return result;
        }
    }
}
