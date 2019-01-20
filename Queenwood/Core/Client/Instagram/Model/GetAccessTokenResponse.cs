using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Core.Client.Instagram.Model
{
    public class GetAccessTokenResponse : APIResult
    {
        public string AccessToken { get; set; }
        public User User { get; set; }
    }
}
