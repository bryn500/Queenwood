﻿using Queenwood.Core.Client.Instagram.Model;
using Queenwood.Models.Shared;
using Queenwood.Models.VIewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Queenwood.Core.Client.Instagram
{
    public interface IInstagramClient
    {
        Task<APIResult> GetAccessToken(string code);
        Task<List<InstagramMedia>> GetRecentMedia();
    }
}
