using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
