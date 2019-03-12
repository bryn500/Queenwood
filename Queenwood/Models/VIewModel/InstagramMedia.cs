using Queenwood.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Models.VIewModel
{
    public class InstagramMedia
    {
        public ImageLink DefaultImage { get; set; }
        public List<Image> Album { get; set; }
    }
}
