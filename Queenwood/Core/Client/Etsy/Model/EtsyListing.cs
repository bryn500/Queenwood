using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Core.Client.Etsy.Model
{
    public class EtsyListing
    {
        public int listing_id { get; set; }
        public string state { get; set; }
        public int user_id { get; set; }
        public int category_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string price { get; set; }
        public string currency_code { get; set; }
        public int quantity { get; set; }
        public string url { get; set; }
        public List<Image> Images { get; set; }
    }
}