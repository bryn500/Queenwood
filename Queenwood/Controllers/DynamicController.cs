using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Queenwood.Core.Services.ContentfulService;

namespace Queenwood.Controllers
{
    [ResponseCache(CacheProfileName = "Default")]
    public class DynamicController : BaseController
    {
        public DynamicController(IContentfulService contentfulService) : base(contentfulService)
        {
        }

        [HttpGet("")]
        [HttpGet("{urlSlug}")]
        public IActionResult DynamicPage(string urlSlug)
        {
            if (urlSlug == null)
                urlSlug = "/";

            if (!_contentfulService.GetContentfulUrls().Contains(urlSlug))
                return NotFound();

            var model = _contentfulService.GetContentfulWebpages().Where(x => x.Urlslug == urlSlug).First();

            if (model.HeaderImage != null)
                model.HeaderImage.LowRes = _contentfulService.GetHeaderImagesAsBase64(model.HeaderImage);

            ViewData.Add("Title", model.SEOTitle);
            ViewData.Add("Description", model.SEODescription);
            ViewData.Add("Keywords", model.SEOKeywords);

            return View(model);
        }

        [HttpGet("/preview/{urlSlug}")]
        public IActionResult DynamicPagePreview(string urlSlug)
        {
            if (!_contentfulService.PreviewContentfulUrls().Contains(urlSlug))
                return NotFound();

            var model = _contentfulService.PreviewContentfulWebpages().Where(x => x.Urlslug == urlSlug).First();

            if (model.HeaderImage != null)
                model.HeaderImage.LowRes = _contentfulService.GetHeaderImagesAsBase64(model.HeaderImage);

            ViewData.Add("Title", model.SEOTitle);
            ViewData.Add("Description", model.SEODescription);
            ViewData.Add("Keywords", model.SEOKeywords);

            return View(model);
        }
    }
}
