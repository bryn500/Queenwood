using Queenwood.Core.Client.Ebay.Model;
using Queenwood.Core.Client.Etsy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Models
{
    public class Products
    {
        // Etsy     
        public Etsy Etsy { get; set; }

        // Ebay
        public Ebay Ebay { get; set; }

        // Instagram
        public Instagram Instagram { get; set; }

        public Products()
        {            
            Etsy = new Etsy();
            Ebay = new Ebay();
            Instagram = new Instagram();
        }
    }

    public class Etsy
    {
        public string ShopUrl { get; set; }
        public List<EtsyListing> ShopListings { get; set; }

        public Etsy()
        {
            ShopListings = new List<EtsyListing>();
        }
    }

    public class Ebay
    {
        public string ShopUrl { get; set; }
        public List<Item> ShopListings { get; set; }

        public Ebay()
        {
            ShopListings = new List<Item>();
        }
    }

    public class Instagram
    {
        public string Url { get; set; }
        public List<ImageLink> PortfolioItems { get; set; }

        public Instagram()
        {
            PortfolioItems = new List<ImageLink>();
        }
    }
}
