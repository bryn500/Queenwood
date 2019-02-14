using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contentful.Core;
using Contentful.Core.Configuration;
using Microsoft.Extensions.Options;
using Queenwood.Models.Config;
using Queenwood.Core.Client;
using Queenwood.Core.Services.CacheService;
using Queenwood.Core.Services.ContentfulService.Model;
using Queenwood.Models.ViewModel;
using Contentful.Core.Search;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Queenwood.Models.Shared;

namespace Queenwood.Core.Services.ContentfulService
{
    public class ContentfulService : IContentfulService
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ICacheService _cacheService;
        private readonly ContentfulConfig _contentfulConfig;
        private readonly string _imageDBPath;

        private readonly ContentfulClient _client;
        private readonly ContentfulClient _previewClient;

        public ContentfulService(IHostingEnvironment hostingEnvironment, ICacheService cacheService, IOptions<ContentfulConfig> contentfulConfig, IBaseClient baseClient)
        {
            _hostingEnvironment = hostingEnvironment;
            _cacheService = cacheService;
            _contentfulConfig = contentfulConfig.Value;

            _imageDBPath = _hostingEnvironment.WebRootPath + "/data/header-images.json";

            // Build Contentful client
            var options = new ContentfulOptions()
            {
                UsePreviewApi = false,
                DeliveryApiKey = _contentfulConfig.DeliveryApiKey,
                SpaceId = _contentfulConfig.SpaceId
            };

            _client = new ContentfulClient(baseClient.GetHttpClient(), options);

            // Build Contentful preview client
            var previewOptions = new ContentfulOptions()
            {
                UsePreviewApi = true,
                PreviewApiKey = _contentfulConfig.PreviewApiKey,
                SpaceId = _contentfulConfig.SpaceId
            };

            _previewClient = new ContentfulClient(baseClient.GetHttpClient(), previewOptions);            
        }


        /// 
        /// Main methods
        ///

        public async Task<List<Webpage>> GetContentfulWebpages()
        {
            return await _cacheService.GetAsync("GetContentfulWebpages", async () =>
            {
                var builder = new QueryBuilder<ContentfulWebpage>().Include(10);

                var response = await _client.GetEntriesByType("webpage", builder);

                return response.Select(x => new Webpage(x)).ToList();
            }, 1440).Unwrap();
        }

        public async Task<List<string>> GetContentfulUrls()
        {
            return await _cacheService.GetAsync("GetContentfulUrls", async () =>
            {
                var webpages = await GetContentfulWebpages();

                return webpages.Select(x => x.Urlslug).ToList();
            }, 1440).Unwrap();
        }

        public async Task<List<EbayCategoryFilter>> GetEbayCategoryFilters()
        {
            return await _cacheService.GetAsync("GetEbayCategoryFilters", async () =>
            {
                var builder = new QueryBuilder<EbayCategoryFilter>();

                var response = await _client.GetEntriesByType("ebayCategories", builder);

                return response.ToList();
            }, 1440).Unwrap();
        }

        public async Task<string> GetHeaderImagesAsBase64(Image image)
        {
            // Read data
            var json = await System.IO.File.ReadAllTextAsync(_imageDBPath);

            // Deserialize
            var data = JsonConvert.DeserializeObject<HeaderImagesDB>(json);

            // look up by url
            var record = data.HeaderImages.FirstOrDefault(x => x.Url == image.Url);

            // if exists
            if (record != null)
            {
                // return base64
                return record.Base64;
            }
            else
            {
                // download low quality webp image
                var resultUrl = $"https:{image.Url}?fm=webp&q=1";

                using (var client = new WebClient())
                {
                    // download
                    byte[] imageBytes = await client.DownloadDataTaskAsync(new Uri(resultUrl));

                    // convert               
                    var base64 = "data:image/webp;base64," + Convert.ToBase64String(imageBytes);

                    // Add new base64 result to list
                    data.HeaderImages.Add(new HeaderImageDB()
                    {
                        Url = image.Url,
                        Base64 = base64
                    });

                    // Convert back to JSON string
                    var newData = JsonConvert.SerializeObject(data);

                    // write back changes to file
                    await System.IO.File.WriteAllTextAsync(_imageDBPath, newData);

                    // return new base 64 url
                    return base64;
                }
            }
        }


        ///
        /// Preview Methods
        /// No caching for preview client results - don't want to reset cache when previewing changes
        /// 

        public async Task<List<Webpage>> PreviewContentfulWebpages()
        {
            var builder = new QueryBuilder<ContentfulWebpage>().Include(10);

            var response = await _previewClient.GetEntriesByType("webpage", builder);

            return response.Select(x => new Webpage(x)).ToList();
        }

        public async Task<List<string>> PreviewContentfulUrls()
        {
            var webpages = await GetContentfulWebpages();

            return webpages.Select(x => x.Urlslug).ToList();
        }


        /// 
        /// Testing Methods
        /// 

        public async Task<ContentfulExampleModel> SearchContentful()
        {
            var builder = new QueryBuilder<dynamic>().Include(10);

            var entries = await _client.GetEntriesByType("webpage", builder);

            var contentfulExampleModel = new ContentfulExampleModel()
            {
                Entries = entries.ToList()
            };

            return contentfulExampleModel;
        }
    }

    public class HeaderImagesDB
    {
        public List<HeaderImageDB> HeaderImages { get; set; }

        public HeaderImagesDB()
        {
            HeaderImages = new List<HeaderImageDB>();
        }
    }

    public class HeaderImageDB
    {
        public string Url { get; set; }
        public string Base64 { get; set; }
    }

    public class ContentfulExampleModel
    {
        public List<dynamic> Entries { get; set; }
    }
}
