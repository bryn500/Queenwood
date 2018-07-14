using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Queenwood.Core.Client.Etsy;
using Queenwood.Core.Services.CacheService;
using Queenwood.Core.Services.EmailService;
using Queenwood.Models;
using Queenwood.Core.Client.Etsy.Consts;

namespace Queenwood.Controllers
{
    [ResponseCache(CacheProfileName = "Default")]
    public class ProductsController : Controller
    {
        private ICacheService _cacheService;
        private IEmailService _emailClient;
        private IEtsyClient _etsyClient;

        public ProductsController(ICacheService cacheService, IEmailService emailClient, IEtsyClient etsyClient)
        {
            _cacheService = cacheService;
            _emailClient = emailClient;
            _etsyClient = etsyClient;
        }

        [HttpGet("products")]
        public IActionResult Products()
        {
            ViewData.Add("Title", "Products");

            var model = _cacheService.Get("products", () =>
            {
                var products = new Products();

                try
                {
                    var results = _etsyClient.GetListings().Result;
                    var shop = results.Data.results.First();

                    products.ShopName = shop.shop_name;
                    products.ShopUrl = shop.url;
                    products.ShopIcon = shop.icon_url_fullxfull;
                    products.ShopImage = shop.image_url_760x100;

                    products.ShopListings =
                        shop.Listings.Where(x => x.state == ListingState.Active || x.state == ListingState.SoldOut) // active listings
                                     .Where(x => x.Images != null && x.Images.Any()) // with images
                                     .ToList();
                }
                catch (Exception ex)
                {
                    _emailClient.SendErrorAlert(ex.ToString());
                }

                products.PortfolioItems.Add(new ImageLink("https://picsum.photos/g/700/700/?random", 700, 700, "/", "Text Here"));
                products.PortfolioItems.Add(new ImageLink("https://picsum.photos/500/700/?random", 500, 700, "/", "Text Here"));
                products.PortfolioItems.Add(new ImageLink("https://picsum.photos/g/600/700/?random", 600, 700, "/", "Text Here"));
                products.PortfolioItems.Add(new ImageLink("https://picsum.photos/700/500/?random", 700, 500, "/", "Text Here"));
                products.PortfolioItems.Add(new ImageLink("https://picsum.photos/700/600/?random", 700, 600, "/", "Text Here"));
                products.PortfolioItems.Add(new ImageLink("https://picsum.photos/g/500/600/?random", 500, 600, "/", "Text Here"));
                products.PortfolioItems.Add(new ImageLink("https://picsum.photos/g/700/700/?random", 700, 700, "/", "Text Here"));
                products.PortfolioItems.Add(new ImageLink("https://picsum.photos/500/700/?random", 500, 700, "/", "Text Here"));
                products.PortfolioItems.Add(new ImageLink("https://picsum.photos/g/600/700/?random", 600, 700, "/", "Text Here"));
                products.PortfolioItems.Add(new ImageLink("https://picsum.photos/700/500/?random", 700, 500, "/", "Text Here"));
                products.PortfolioItems.Add(new ImageLink("https://picsum.photos/700/600/?random", 700, 600, "/", "Text Here"));
                products.PortfolioItems.Add(new ImageLink("https://picsum.photos/g/500/600/?random", 500, 600, "/", "Text Here"));

                return products;
            }, 60);

            return View(model);
        }
    }
}
