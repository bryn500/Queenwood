using Microsoft.Extensions.Options;
using Queenwood.Core.Client.Instagram.Model;
using Queenwood.Core.Services.CacheService;
using Queenwood.Models.Config;
using Queenwood.Models.Shared;
using Queenwood.Models.VIewModel;
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
        private readonly ICacheService _cacheService;

        public InstagramClient(IOptions<InstagramConfig> instagramConfig, ICacheService cacheService, IBaseClient baseClient) : base(baseClient)
        {
            _instagramConfig = instagramConfig.Value;
            _cacheService = cacheService;
        }

        public async Task<APIResult> GetAccessToken(string code)
        {
            var payload = new Dictionary<string, string>();
            payload.Add("client_id", _instagramConfig.ClientId);
            payload.Add("client_secret", _instagramConfig.ClientSecret);
            payload.Add("grant_type", "authorization_code");
            payload.Add("redirect_uri", _instagramConfig.RedirectUrl);
            payload.Add("code", code);

            return await PostFormUrlAsync(_instagramConfig.BaseUrl, $"/oauth/access_token", payload);
        }

        public async Task<List<InstagramMedia>> GetRecentMedia()
        {
            return await _cacheService.GetAsync("GetRecentInstagramMedia", async () =>
            {
                NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
                queryString.Add("access_token", _instagramConfig.AccessToken);

                // Get instagram images
                var instagramResults = await GetAsync<GetRecentMediaResponse>(_instagramConfig.BaseUrl, $"/v1/users/self/media/recent/?{queryString}");

                List<InstagramMedia> items = null;

                if (instagramResults != null)
                {
                    items = new List<InstagramMedia>();

                    foreach (var result in instagramResults.Data.Data)
                    {
                        if (result.Images != null)
                        {
                            var media = new InstagramMedia
                            {
                                DefaultImage = new ImageLink(
                                    result.Images.StandardResolution.Url,
                                    result.Images.StandardResolution.Width,
                                    result.Images.StandardResolution.Height,
                                    result.Link, "")
                                {
                                    LowRes = result.Images.LowResolution.Url,
                                    LinkText = result.Caption?.Text
                                }
                            };

                            if (result.CarouselMedia != null && result.CarouselMedia.Any())
                            {
                                // album
                                media.Album = result.CarouselMedia.Where(x => x.Images != null)
                                    .Select(x => new Image(
                                        x.Images.StandardResolution.Url,
                                        x.Images.StandardResolution.Width,
                                        x.Images.StandardResolution.Height)
                                    {
                                        LowRes = x.Images.LowResolution.Url
                                    })
                                    .ToList();
                            }

                            items.Add(media);
                        }
                    }

                    // Remove Hashtags and ..... from instagram comments
                    foreach (var p in items)
                    {
                        if (string.IsNullOrEmpty(p.DefaultImage.LinkText))
                            continue;

                        // Manage text
                        p.DefaultImage.LinkText = RemoveInstagramQuirksFromString(p.DefaultImage.LinkText);
                        p.DefaultImage.LinkText = Utilities.ReduceMultipleSpaces(p.DefaultImage.LinkText);
                        p.DefaultImage.LinkText = RemoveHashTagFromString(p.DefaultImage.LinkText);
                    }
                }

                return items;

            }, 1440).Unwrap();
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
