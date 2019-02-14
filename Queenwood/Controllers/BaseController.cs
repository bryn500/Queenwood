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
        protected readonly IContentfulService _contentfulService;

        public BaseController(IContentfulService contentfulService)
        {
            _contentfulService = contentfulService;
        }

        public override async void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            await SetNavLinks();
        }

        private async Task SetNavLinks()
        {
            var webpages = await _contentfulService.GetContentfulWebpages();

            ViewBag.NavLinks = webpages.Where(x => x.InNav == true)
                .Select(x => new KeyValuePair<string, string>("/" + x.Urlslug.TrimStart('/'), x.Title))
                .ToList();
        }
    }
}
