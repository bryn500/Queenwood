using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Models.Config
{
    public class EmailConfig
    {
        public string EmailFrom { get; set; }
        public string ErrorEmail { get; set; }
        public string SmtpClientHost { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPassword { get; set; }
    }
}
