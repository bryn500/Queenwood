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
        public string ShopName { get; set; }
        public string ShopUrl { get; set; }
        public string ShopMessage { get; set; }
        public string ShopIcon { get; set; }
        public string ShopImage { get; set; }
        public List<Listing> ShopListings { get; set; }

        // Portfolio
        public List<ImageLink> PortfolioItems { get; set; }

        public Products()
        {
            ShopListings = new List<Listing>();
            PortfolioItems = new List<ImageLink>();
        }
    }
}
