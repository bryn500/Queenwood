using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Core.Client.Etsy.Model
{
    public class EtsyAPICall
    {
        public int count { get; set; }
        public List<Shop> results { get; set; }
        public string type { get; set; }
    }
}