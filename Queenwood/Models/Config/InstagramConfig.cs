using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Models.Config
{
    public class InstagramConfig
    {
        public string BaseUrl { get; set; }
        public string RedirectUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AccessToken { get; set; }
    }
}
