using Contentful.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Core.Services.ContentfulService.Model
{
    public class ContentfulWebpage
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("seoTitle")]
        public string SEOTitle { get; set; }

        [JsonProperty("seoDescription")]
        public string SEODescription { get; set; }

        [JsonProperty("seoKeywords")]
        public string SEOKeywords { get; set; }

        [JsonProperty("urlslug")]
        public string Urlslug { get; set; }

        [JsonProperty("inNav")]
        public bool InNav { get; set; }
        
        [JsonProperty("headerImage")]
        public HeaderImage HeaderImage { get; set; }

        [JsonProperty("pageContent")]
        public List<ContentfulContent> PageContent { get; set; }
    }

    public class HeaderImage
    {
        [JsonProperty("$id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("image")]
        public ContentfulImage Image { get; set; }

        [JsonProperty("overlayText")]
        public string OverlayText { get; set; }
    }

    public class ContentfulContent : GalleryImage
    {
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public Document Document { get; set; }

        [JsonProperty("imageLink", NullValueHandling = NullValueHandling.Ignore)]
        public List<GalleryImage> Gallery { get; set; }
    }

    public class GalleryImage
    {
        [JsonProperty("$id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public ContentfulImage Image { get; set; }

        [JsonProperty("linkUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("linkText", NullValueHandling = NullValueHandling.Ignore)]
        public string LinkText { get; set; }
    }

    public class ContentfulImage
    {
        [JsonProperty("$id")]
        public string Id { get; set; }

        [JsonProperty("fields")]
        public Fields Fields { get; set; }
    }

    public class Fields
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("file")]
        public File File { get; set; }
    }

    public class File
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("details")]
        public FileDetails Details { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }
    }

    public class FileDetails
    {
        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("image")]
        public FileDimensions Dimensions { get; set; }
    }

    public class FileDimensions
    {
        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
}