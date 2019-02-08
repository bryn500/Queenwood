﻿using Queenwood.Core.Services.ContentfulService.Model;
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
        List<EbayCategoryFilter> GetEbayCategoryFilters();
        Task<ContentfulExampleModel> SearchContentful();
        string GetHeaderImagesAsBase64(Image image);
    }
}
