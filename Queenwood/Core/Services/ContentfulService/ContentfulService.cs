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
        private static readonly object _imageLocker = new object();

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ICacheService _cacheService;
        private readonly ContentfulConfig _contentfulConfig;
        private readonly ContentfulClient _client;
        private readonly ContentfulClient _previewClient;


        private readonly string _imageDBPath;

        public ContentfulService(IHostingEnvironment hostingEnvironment, ICacheService cacheService, IOptions<ContentfulConfig> contentfulConfig, IBaseClient baseClient)
        {
            _hostingEnvironment = hostingEnvironment;
            _cacheService = cacheService;

            _contentfulConfig = contentfulConfig.Value;

            var options = new ContentfulOptions()
            {
                UsePreviewApi = false,
                DeliveryApiKey = _contentfulConfig.DeliveryApiKey,
                SpaceId = _contentfulConfig.SpaceId
            };

            _client = new ContentfulClient(baseClient.GetHttpClient(), options);

            var previewOptions = new ContentfulOptions()
            {
                UsePreviewApi = true,
                PreviewApiKey = _contentfulConfig.PreviewApiKey,
                SpaceId = _contentfulConfig.SpaceId
            };

            _previewClient = new ContentfulClient(baseClient.GetHttpClient(), previewOptions);

            _imageDBPath = _hostingEnvironment.WebRootPath + "/data/header-images.json";
        }


        /// 
        /// Main methods
        ///

        public List<Webpage> GetContentfulWebpages()
        {
            return _cacheService.Get("GetContentfulWebpages", () =>
            {
                var builder = new QueryBuilder<ContentfulWebpage>().Include(10);

                var response = _client.GetEntriesByType("webpage", builder).Result.ToList();

                return response.Select(x => new Webpage(x)).ToList();
            }, 1440);
        }

        public List<string> GetContentfulUrls()
        {
            return _cacheService.Get("GetContentfulUrls", () =>
            {
                var webpages = GetContentfulWebpages();

                return webpages.Select(x => x.Urlslug).ToList();
            }, 1440);
        }

        public List<EbayCategoryFilter> GetEbayCategoryFilters()
        {
            return _cacheService.Get("GetEbayCategoryFilters", () =>
            {
                var builder = new QueryBuilder<EbayCategoryFilter>();

                var response = _client.GetEntriesByType("ebayCategories", builder).Result.ToList();

                return response.ToList();
            }, 1440);
        }        

        public string GetHeaderImagesAsBase64(Image image)
        {
            // lock open file
            lock (_imageLocker)
            {
                // Read data
                var json = System.IO.File.ReadAllText(_imageDBPath);

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
                        byte[] imageBytes = client.DownloadData(resultUrl);

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
                        System.IO.File.WriteAllText(_imageDBPath, newData);

                        // return new base 64 url
                        return base64;
                    }
                }
            }
        }

        ///
        /// Preview Methods
        /// No caching for preview client results - don't want to reset cache when previewing changes
        /// 

        public List<Webpage> PreviewContentfulWebpages()
        {            
            var builder = new QueryBuilder<ContentfulWebpage>().Include(10);

            var response = _previewClient.GetEntriesByType("webpage", builder).Result.ToList();

            return response.Select(x => new Webpage(x)).ToList();
        }

        public List<string> PreviewContentfulUrls()
        {
            var webpages = GetContentfulWebpages();

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
