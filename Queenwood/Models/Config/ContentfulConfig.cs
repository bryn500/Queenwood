using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Models.Config
{
    public class ContentfulConfig
    {
        public string DeliveryApiKey { get; set; }
        public string PreviewApiKey { get; set; }
        public string ManagementApiKey { get; set; }
        public string SpaceId { get; set; }
        public bool UsePreviewApi { get; set; }
    }
}
