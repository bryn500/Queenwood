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
        public async Task<IActionResult> DynamicPage(string urlSlug)
        {
            if (urlSlug == null)
                urlSlug = "/";

            var validUrls = await _contentfulService.GetContentfulUrls();

            if (!validUrls.Contains(urlSlug))
                return NotFound();

            var webpages = await _contentfulService.GetContentfulWebpages();

            var model = webpages.Where(x => x.Urlslug == urlSlug).First();

            if (model.HeaderImage != null)
                model.HeaderImage.LowRes = await _contentfulService.GetHeaderImagesAsBase64(model.HeaderImage);

            ViewData.Add("Title", model.SEOTitle);
            ViewData.Add("Description", model.SEODescription);
            ViewData.Add("Keywords", model.SEOKeywords);

            return View(model);
        }

        [HttpGet("/preview/{urlSlug}")]
        public async Task<IActionResult> DynamicPagePreview(string urlSlug)
        {
            var validUrls = await _contentfulService.PreviewContentfulUrls();

            if (!validUrls.Contains(urlSlug))
                return NotFound();

            var previewWebpages = await _contentfulService.PreviewContentfulWebpages();

            var model = previewWebpages.Where(x => x.Urlslug == urlSlug).First();

            if (model.HeaderImage != null)
                model.HeaderImage.LowRes = await _contentfulService.GetHeaderImagesAsBase64(model.HeaderImage);

            ViewData.Add("Title", model.SEOTitle);
            ViewData.Add("Description", model.SEODescription);
            ViewData.Add("Keywords", model.SEOKeywords);

            return View(model);
        }
    }
}
