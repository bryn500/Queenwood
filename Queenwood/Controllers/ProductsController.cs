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
using System.Linq;

namespace Queenwood.Controllers
{
    [ResponseCache(CacheProfileName = "Default")]
    public class ProductsController : BaseController
    {
        private ICacheService _cacheService;
        private IEmailService _emailService;
        private IEtsyClient _etsyClient;
        private IEbayClient _ebayClient;
        private IInstagramClient _instagramClient;

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
        public IActionResult Products()
        {
            ViewData.Add("Title", "Products");
            ViewData.Add("Description", "Items for sale, past portfolio");
            ViewData.Add("Keywords", "Products, For sale, portfolio");

            var model = _cacheService.Get("products", () =>
            {
                var products = new Products();

                // todo: Figure out how to fire all async tasks at same time using same instance of httpclient and process in order of completion

                // etsy
                var etsyCall = _etsyClient.GetListings();

                try
                {
                    products.Etsy.ShopUrl = Consts.EtsyUrl;
                    products.Etsy.ShopListings = _etsyClient.ProcessListings(etsyCall);
                }
                catch (Exception ex)
                {
                    _emailService.SendErrorAlert(ex.ToString());
                }

                // ebay
                var ebayCall = _ebayClient.GetUserListings(_ebayConfig.UserId);

                try
                {
                    products.Ebay.ShopUrl = Consts.EbayUrl;
                    products.Ebay.ShopListings = _ebayClient.ProcessListings(ebayCall);
                }
                catch (Exception ex)
                {
                    _emailService.SendErrorAlert(ex.ToString());
                }

                // instagram
                var instagramCall = _instagramClient.GetRecentMedia();

                try
                {
                    products.Instagram.Url = Consts.InstagramUrl;
                    products.Instagram.PortfolioItems = _instagramClient.ProcessRecentMediaResult(instagramCall);
                }
                catch (Exception ex)
                {
                    _emailService.SendErrorAlert(ex.ToString());
                }

                return products;
            }, 60);

            return View(model);
        }
    }
}
