using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Core.Client.Instagram.Model
{
    public partial class User
    {
        public long id { get; set; }
        public string type { get; set; }
        public string full_name { get; set; }
        public string profile_picture { get; set; }
        public string username { get; set; }
    }
}
