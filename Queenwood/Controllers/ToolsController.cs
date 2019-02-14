using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Queenwood.Core.Services.CacheService;
using Queenwood.Core.Services.EmailService;

namespace Queenwood.Controllers
{
    [Route("tools")]
    public class ToolsController : Controller
    {
        private readonly ICacheService _cacheService;
        private readonly IEmailService _emailService;

        public ToolsController(ICacheService cacheService, IEmailService emailService)
        {
            _cacheService = cacheService;
            _emailService = emailService;
        }

        [HttpGet("resetcache")]
        public ActionResult ResetCache()
        {
            _cacheService.RemoveAll();

            return Content("OK");
        }

        [HttpGet("problem")]
        public IActionResult Problem()
        {
            try
            {
                throw new Exception("test");
            }
            catch (Exception ex)
            {
                _emailService.SendErrorAlert(ex.ToString());
            }

            return StatusCode(500);
        }
    }
}
