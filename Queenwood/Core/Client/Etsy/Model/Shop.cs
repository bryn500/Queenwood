using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Core.Client.Etsy.Model
{
    public class Shop
    {
        public int shop_id { get; set; }
        public string shop_name { get; set; }
        public int listing_active_count { get; set; }
        public string url { get; set; }
        public string image_url_760x100 { get; set; }
        public string icon_url_fullxfull { get; set; }
        public string announcement { get; set; }
        public bool is_vacation { get; set; }
        public string vacation_message { get; set; }

        public List<Listing> Listings { get; set; }
    }
}