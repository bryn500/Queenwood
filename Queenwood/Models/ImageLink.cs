using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Models
{
    public class ImageLink : Image
    {
        public string LinkUrl { get; set; }
        public string LinkText { get; set; }

        public ImageLink(string url, int width, int height, string linkUrl, string linkText)
            : base(url, width, height)
        {
            LinkUrl = linkUrl;
            LinkText = linkText;
        }
    }
}

