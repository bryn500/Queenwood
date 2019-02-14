using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Queenwood.Core.Services.ContentfulService;
using Queenwood.Core.Services.EmailService;
using Queenwood.Models.ViewModel;

namespace Queenwood.Controllers
{
    [ResponseCache(CacheProfileName = "Default")]
    public class ContactController : BaseController
    {
        private readonly IEmailService _emailService;

        public ContactController(IEmailService emailService, IContentfulService contentfulService) : base(contentfulService)
        {
            _emailService = emailService;
        }

        [HttpGet("contact")]
        [ResponseCache(CacheProfileName = "Never")]
        public IActionResult Contact(bool success)
        {
            var model = new Contact()
            {
                Success = success
            };

            return View(model);
        }

        [HttpPost]
        [Route("contact")]
        [ValidateAntiForgeryToken]
        [ResponseCache(CacheProfileName = "Never")]
        public async Task<IActionResult> Contact(Contact model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var message = $"{model.Name} - {model.Email}{Environment.NewLine} {model.Message}";

            var result = await _emailService.SendEnquiry(model.Subject, message);

            return RedirectToAction("Contact", new { success = true });
        }
    }
}
