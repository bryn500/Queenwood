using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Queenwood.Core.Services.ContentfulService;

namespace Queenwood.Controllers
{
    [Route("error")]
    public class ErrorController : BaseController
    {
        public ErrorController(IContentfulService contentfulService) : base(contentfulService)
        {
        }

        [Route("404")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [Route("500")]
        public IActionResult ServerError()
        {
            return View();
        }
    }
}
