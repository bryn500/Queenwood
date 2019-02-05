using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Queenwood.Core.Client.Instagram;
using Queenwood.Core.Services.ContentfulService;
using Queenwood.Models.Config;
using Queenwood.Models.ViewModel;

namespace Queenwood.Controllers
{
    [Route("instagram")]
    public class InstagramController : BaseController
    {
        private readonly InstagramConfig _instagramConfig;
        private IInstagramClient _client;

        public InstagramController(IOptions<InstagramConfig> instagramConfig, IInstagramClient client, IContentfulService contentfulService) : base(contentfulService)
        {
            _instagramConfig = instagramConfig.Value;
            _client = client;
        }

        [HttpGet("authorize")]
        public IActionResult Authorize()
        {
            string InstagramAuthoriseUrl = $"/oauth/authorize/?client_id={_instagramConfig.ClientId}&redirect_uri={HttpUtility.UrlEncode(_instagramConfig.RedirectUrl)}&response_type=code";

            return Redirect(_instagramConfig.BaseUrl + InstagramAuthoriseUrl);
        }    

        [HttpGet("authorize-response")]
        public IActionResult AuthorizeResponse(string code)
        {
            if (String.IsNullOrEmpty(code))
            {
                var model = new InstagramDone()
                {
                    Error = Request.Query["error"].ToString(),
                    ErrorReason = Request.Query["error_reason"].ToString(),
                    ErrorDescription = Request.Query["error_description"].ToString()
                };

                return Json(model);
            }
            else
            {
                var accessToken = _client.GetAccessToken(code).Result;

                return Content(accessToken.Json);
            }
        }
    }
}