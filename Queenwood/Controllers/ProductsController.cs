using System;
using Microsoft.AspNetCore.Mvc;
using Queenwood.Core.Client.Etsy;
using Queenwood.Core.Services.CacheService;
using Queenwood.Core.Services.EmailService;
using Queenwood.Models;
using Queenwood.Core.Client.Instagram;
using Queenwood.Core;
using Queenwood.Core.Client.Ebay;
using Queenwood.Models.Config;
using Microsoft.Extensions.Options;

namespace Queenwood.Controllers
{
    [ResponseCache(CacheProfileName = "Default")]
    public class ProductsController : Controller
    {
        private ICacheService _cacheService;
        private IEmailService _emailClient;
        private IEtsyClient _etsyClient;
        private IEbayClient _ebayClient;
        private IInstagramClient _instagramClient;

        private readonly EbayConfig _ebayConfig;

        public ProductsController(ICacheService cacheService, IEmailService emailClient, IEtsyClient etsyClient, IEbayClient ebayClient, IOptions<EbayConfig> ebayConfig, IInstagramClient instagramClient)
        {
            _cacheService = cacheService;
            _emailClient = emailClient;
            _etsyClient = etsyClient;
            _ebayClient = ebayClient;
            _ebayConfig = ebayConfig.Value;
            _instagramClient = instagramClient;
        }

        [HttpGet("products")]
        public IActionResult Products()
        {
            ViewData.Add("Title", "Products");

            var model = _cacheService.Get("products", () =>
            {
                var products = new Products();

                var instagramCall = _instagramClient.GetRecentMedia();
                var etsyCall = _etsyClient.GetListings();
                var ebayCall = _ebayClient.GetUserListings(_ebayConfig.UserId);

                // etsy
                try
                {
                    products.Etsy.ShopUrl = Consts.EtsyUrl;
                    products.Etsy.ShopListings = _etsyClient.ProcessListings(etsyCall);
                }
                catch (Exception ex)
                {
                    _emailClient.SendErrorAlert(ex.ToString());
                }

                // ebay
                try
                {
                    products.Ebay.ShopUrl = Consts.EbayUrl;
                    products.Ebay.ShopListings = _ebayClient.ProcessListings(ebayCall).ItemArray.Items;
                }
                catch (Exception ex)
                {
                    _emailClient.SendErrorAlert(ex.ToString());
                }

                // instagram
                try
                {
                    products.Instagram.Url = Consts.InstagramUrl;
                    products.Instagram.PortfolioItems = _instagramClient.ProcessRecentMediaResult(instagramCall);
                }
                catch (Exception ex)
                {
                    _emailClient.SendErrorAlert(ex.ToString());
                }

                return products;
            }, 60);

            return View(model);
        }
    }
}
