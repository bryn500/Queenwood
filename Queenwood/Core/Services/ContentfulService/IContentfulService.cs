using Queenwood.Models.Shared;
using Queenwood.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Core.Services.ContentfulService
{
    public interface IContentfulService
    {
        List<Webpage> GetContentfulWebpages();
        List<string> GetContentfulUrls();
        Task<ContentfulExampleModel> SearchContentful();
        string GetHeaderImagesAsBase64(Image image);
    }
}
