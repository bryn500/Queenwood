using Queenwood.Core.Services.ContentfulService.Model;
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
        // Main
        Task<List<Webpage>> GetContentfulWebpages();
        Task<List<string>> GetContentfulUrls();
        Task<List<EbayCategoryFilter>> GetEbayCategoryFilters();
        Task<string> GetHeaderImagesAsBase64(Image image);

        // Preview
        Task<List<Webpage>> PreviewContentfulWebpages();
        Task<List<string>> PreviewContentfulUrls();

        // Testing
        Task<ContentfulExampleModel> SearchContentful();
    }
}
