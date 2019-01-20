using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Core.Client.Instagram.Model
{
    public class GetRecentMediaResponse
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("data")]
        public List<InstagramPost> Data { get; set; }

        [JsonProperty("meta")]
        public object Meta { get; set; }
    }

    public class Pagination
    {
    }

    public class InstagramPost
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("images")]
        public Images Images { get; set; }

        [JsonProperty("created_time")]
        public int CreatedTime { get; set; }

        [JsonProperty("caption")]
        public Caption Caption { get; set; }

        [JsonProperty("user_has_liked")]
        public bool UserHasLiked { get; set; }

        [JsonProperty("likes")]
        public Counter Likes { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("filter")]
        public string Filter { get; set; }

        [JsonProperty("comments")]
        public Counter Comments { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("location")]
        public object Location { get; set; }

        [JsonProperty("attribution")]
        public object Attribution { get; set; }

        [JsonProperty("users_in_photo")]
        public List<object> UsersInPhoto { get; set; }

        [JsonProperty("carousel_media", NullValueHandling = NullValueHandling.Ignore)]
        public List<CarouselMedia> CarouselMedia { get; set; }
    }

    public class MediaFile
    {
        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class Images
    {
        [JsonProperty("thumbnail")]
        public MediaFile Thumbnail { get; set; }

        [JsonProperty("low_resolution")]
        public MediaFile LowResolution { get; set; }

        [JsonProperty("standard_resolution")]
        public MediaFile StandardResolution { get; set; }
    }

    public class Caption
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("created_time")]
        public int CreatedTime { get; set; }

        [JsonProperty("from")]
        public User From { get; set; }
    }

    public class CarouselMedia
    {
        [JsonProperty("images")]
        public Images Images { get; set; }

        [JsonProperty("users_in_photo")]
        public List<object> UsersInPhoto { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Location
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("street_address")]
        public string StreetAddress { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Counter
    {
        [JsonProperty("count")]
        public long Count { get; set; }
    }
}
