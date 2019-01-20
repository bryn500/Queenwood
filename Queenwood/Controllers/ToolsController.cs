using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Queenwood.Core.Services.CacheService;

namespace Queenwood.Controllers
{
    [Route("tools")]
    public class ToolsController : Controller
    {
        private readonly ICacheService cacheService;

        public ToolsController(ICacheService cacheService)
        {
            this.cacheService = cacheService;
        }

        [HttpGet("resetcache")]
        public ActionResult ResetCache()
        {
            cacheService.RemoveAll();

            return Content("OK");
        }

        [HttpGet("problem")]
        public IActionResult Problem()
        {
            return StatusCode(500);
        }
    }
}
