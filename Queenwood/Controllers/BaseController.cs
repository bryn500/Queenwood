using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Queenwood.Core.Services.ContentfulService;

namespace Queenwood.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IContentfulService _contentfulService;

        public BaseController(IContentfulService contentfulService)
        {
            _contentfulService = contentfulService;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            ViewBag.NavLinks = _contentfulService
                .GetContentfulWebpages().Where(x => x.InNav == true)
                .Select(x => new KeyValuePair<string, string>("/" + x.Urlslug.TrimStart('/'), x.Title))
                .ToList();
        }
    }
}
