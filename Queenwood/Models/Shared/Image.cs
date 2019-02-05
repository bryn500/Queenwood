using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Models.Shared
{
    public class Image : IImage
    {
        public string Url { get; set; }
        public string LowRes { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Alt { get; set; }

        public double AspectRatioPercentage => ((double)Height / Width) * 100;

        public Image(string url, int width, int height)
        {
            Url = url;
            Width = width;
            Height = height;
        }
    }

    public interface IImage
    {

    }
}
