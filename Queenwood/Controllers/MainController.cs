using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Queenwood.Core.Services.EmailService;
using Queenwood.Models;

namespace Queenwood.Controllers
{
    [ResponseCache(CacheProfileName = "Default")]
    public class MainController : Controller
    {
        private IEmailService _emailService;

        public MainController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewData.Add("Title", "Home");

            return View();
        }

        [HttpGet("/about")]
        public IActionResult About()
        {
            ViewData.Add("Title", "About");

            return View();
        }

        [HttpGet("bespoke")]
        [ResponseCache(CacheProfileName = "Never")]
        public IActionResult Bespoke(bool success)
        {
            var model = new Contact()
            {
                Success = success
            };

            return View(model);
        }

        [HttpPost]
        [Route("bespoke")]
        [ValidateAntiForgeryToken]
        [ResponseCache(CacheProfileName = "Never")]
        public async Task<IActionResult> Bespoke(Contact model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var message = $"{model.FirstName} {model.LastName} {model.Email}{Environment.NewLine} {model.Message}";

            var result = await _emailService.SendEnquiry(model.Subject, message);

            return RedirectToAction("Bespoke", new { success = true });
        }
    }
}
