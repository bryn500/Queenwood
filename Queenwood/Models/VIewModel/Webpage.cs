using Contentful.Core.Models;
using Newtonsoft.Json;
using Queenwood.Core;
using Queenwood.Core.Services.ContentfulService.Model;
using Queenwood.Models.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queenwood.Models.ViewModel
{
    public class Webpage : ContentfulViewModel
    {
        public string Title { get; set; }
        public string SEOTitle { get; set; }
        public string SEODescription { get; set; }
        public string SEOKeywords { get; set; }
        public string Urlslug { get; set; }
        public bool InNav { get; set; }
        public bool Published { get; set; }
        public string HeaderImageText { get; set; }
        public Image HeaderImage { get; set; }
        public List<PageContent> PageContent { get; set; }

        public Webpage(ContentfulWebpage data)
        {
            Title = DynamicTextReplacement(data.Title);
            SEOTitle = DynamicTextReplacement(data.SEOTitle);
            SEODescription = DynamicTextReplacement(data.SEODescription);
            SEOKeywords = DynamicTextReplacement(data.SEOKeywords);

            Urlslug = data.Urlslug;
            InNav = data.InNav;

            if (data.HeaderImage != null)
            {
                var i = data.HeaderImage.Image.Fields;

                HeaderImageText = DynamicTextReplacement(data.HeaderImage.OverlayText);
                HeaderImage = (Image)GetImageFromContentfulData(i);
            }

            if (data.PageContent != null)
            {
                PageContent = data.PageContent.Select(x => new PageContent(x)).ToList();
            }
        }
    }

    public class PageContent : ContentfulViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<IImage> Gallery { get; set; }
        public IImage Image { get; set; }
        public string Text { get; set; }

        public PageContent(ContentfulContent data)
        {
            Id = data.Id;
            Title = data.Title;

            if (data.Image != null)
                Image = GetImageFromContentfulData(data.Image.Fields, data.Url, data.LinkText);

            if (data.Document != null)
            {
                var htmlRenderer = new HtmlRenderer();

                var text = htmlRenderer.ToHtml(data.Document).Result;

                Text = DynamicTextReplacement(text);
            }

            if (data.Gallery != null)
            {
                Gallery = new List<IImage>();

                foreach (var item in data.Gallery)
                {
                    Gallery.Add(GetImageFromContentfulData(item.Image.Fields, item.Url, item.LinkText));
                }
            }
        }
    }

    public class ContentfulViewModel
    {
        protected IImage GetImageFromContentfulData(Fields img, string url = null, string linkText = null)
        {
            if (!String.IsNullOrEmpty(url))
                return new ImageLink(img.File.Url, img.File.Details.Dimensions.Width, img.File.Details.Dimensions.Height, url, linkText)
                {
                    Alt = img.Description
                };
            else
                return new Image(img.File.Url, img.File.Details.Dimensions.Width, img.File.Details.Dimensions.Height)
                {
                    Alt = img.Description
                };
        }

        protected string DynamicTextReplacement(string text)
        {
            if (text != null)
            {
                text = text.Replace("#BrandName#", Consts.BrandName);
            }

            return text;
        }
    }
}
