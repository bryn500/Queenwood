using Microsoft.Extensions.Options;
using Queenwood.Core.Client.Instagram.Model;
using Queenwood.Models;
using Queenwood.Models.Config;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Queenwood.Core.Client.Instagram
{
    public class InstagramClient : APIClient, IInstagramClient
    {
        private readonly InstagramConfig _instagramConfig;

        public InstagramClient(IOptions<InstagramConfig> instagramConfig)
        {
            _instagramConfig = instagramConfig.Value;
        }

        public async Task<APIResult> GetAccessToken(string code)
        {
            var payload = new Dictionary<string, string>();
            payload.Add("client_id", _instagramConfig.ClientId);
            payload.Add("client_secret", _instagramConfig.ClientSecret);
            payload.Add("grant_type", "authorization_code");
            payload.Add("redirect_uri", _instagramConfig.RedirectUrl);
            payload.Add("code", code);

            return await PostFormUrl(_instagramConfig.BaseUrl, $"/oauth/access_token", payload);
        }

        public async Task<APIResult<GetRecentMediaResponse>> GetRecentMedia()
        {
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add("access_token", _instagramConfig.AccessToken);

            return await Get<GetRecentMediaResponse>(_instagramConfig.BaseUrl, $"/v1/users/self/media/recent/?{queryString}");
        }

        public List<ImageLink> ProcessRecentMediaResult(Task<APIResult<GetRecentMediaResponse>> apiResult)
        {
            // Get instagram images
            var instagramResults = apiResult.Result;

            List<ImageLink> items = null;

            if (instagramResults != null)
            {
                items  = new List<ImageLink>();

                items = instagramResults.Data.Data
                    .Where(x => x.Images != null)
                    .Select(x => new ImageLink(
                        x.Images.StandardResolution.Url,
                        x.Images.StandardResolution.Width,
                        x.Images.StandardResolution.Height,
                        x.Link, "")
                    {
                        LowRes = x.Images.LowResolution.Url,
                        LinkText = x.Caption?.Text
                    })
                    .ToList();

                // Remove Hashtags and ...
                foreach (var p in items)
                {
                    if (string.IsNullOrEmpty(p.LinkText))
                        continue;

                    // Manage text
                    p.LinkText = RemoveInstagramQuirksFromString(p.LinkText);
                    p.LinkText = Utilities.ReduceMultipleSpaces(p.LinkText);
                    p.LinkText = RemoveHashTagFromString(p.LinkText);

                }                
            }

            return items;
        }

        private string RemoveInstagramQuirksFromString(string s)
        {
            s = s.Replace(Environment.NewLine + ".", " ");
            s = s.Replace("\n.", " ");

            return s;
        }

        private string RemoveHashTagFromString(string s)
        {
            var words = s.Split(' ');

            return string.Join(' ', words.Where(x => x.First() != '#'));
        }
    }
}
