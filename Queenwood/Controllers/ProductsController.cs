using System;
using Microsoft.AspNetCore.Mvc;
using Queenwood.Core.Client.Etsy;
using Queenwood.Core.Services.CacheService;
using Queenwood.Core.Services.EmailService;
using Queenwood.Core.Client.Instagram;
using Queenwood.Core;
using Queenwood.Core.Client.Ebay;
using Queenwood.Models.Config;
using Microsoft.Extensions.Options;
using Queenwood.Models.ViewModel;
using Queenwood.Core.Services.ContentfulService;
using System.Threading.Tasks;

namespace Queenwood.Controllers
{
    [ResponseCache(CacheProfileName = "Default")]
    public class ProductsController : BaseController
    {
        private readonly ICacheService _cacheService;
        private readonly IEmailService _emailService;
        private readonly IEtsyClient _etsyClient;
        private readonly IEbayClient _ebayClient;
        private readonly IInstagramClient _instagramClient;

        private readonly EbayConfig _ebayConfig;

        public ProductsController(
            ICacheService cacheService, IEmailService emailService, IEtsyClient etsyClient, IEbayClient ebayClient,
            IOptions<EbayConfig> ebayConfig, IInstagramClient instagramClient, IContentfulService contentfulService)
            : base(contentfulService)
        {
            _cacheService = cacheService;
            _emailService = emailService;
            _etsyClient = etsyClient;
            _ebayClient = ebayClient;
            _ebayConfig = ebayConfig.Value;
            _instagramClient = instagramClient;
        }

        [HttpGet("products")]
        public async Task<IActionResult> Products()
        {
            ViewData.Add("Title", "Products");
            ViewData.Add("Description", "Items for sale, past portfolio");
            ViewData.Add("Keywords", "Products, For sale, portfolio");

            var model = await _cacheService.GetAsync("products", async () =>
            {
                var products = new Products();

                try
                {
                    var etsyTask = _etsyClient.GetListings();
                    var ebayTask = _ebayClient.GetUserListings(_ebayConfig.UserId);
                    var instagramTask = _instagramClient.GetRecentMedia();

                    products.Etsy.ShopListings = await etsyTask;
                    products.Ebay.ShopListings = await ebayTask;
                    products.Instagram.PortfolioItems = await instagramTask;

                    products.Etsy.ShopUrl = Consts.EtsyUrl;
                    products.Ebay.ShopUrl = Consts.EbayUrl;
                    products.Instagram.Url = Consts.InstagramUrl;
                }
                catch (Exception ex)
                {
                    _emailService.SendErrorAlert(ex.ToString());
                }

                return products;
            }, 60).Unwrap();

            return View(model);
        }
    }
}
