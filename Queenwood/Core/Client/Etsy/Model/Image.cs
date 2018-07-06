using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Core.Client.Etsy.Model
{
    public class Image
    {
        public int listing_image_id { get; set; }
        public int listing_id { get; set; }
        public string url_75x75 { get; set; }
        public string url_170x135 { get; set; }
        public string url_570xN { get; set; }
        public string url_fullxfull { get; set; }
        public int full_height { get; set; }
        public int full_width { get; set; }

        public double AspectRatioPercentage => ((double)full_height / full_width) * 100;
    }
}
