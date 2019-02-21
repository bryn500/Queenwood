using Microsoft.AspNetCore.Http;

namespace Queenwood.Core
{
    public static class Urls
    {
        public const string HostedContentBaseUrl = "https://testcontent.azureedge.net/qwd";

        public static string GetHostedContentUrl(HttpRequest request, string relativePath)
        {
            // remove leading slash
            relativePath = relativePath.TrimStart('/');

            var localPath = $"/{relativePath}";

            // use locally stored content for development
            if (request.IsLocal())
                return localPath;
            else
                return $"{HostedContentBaseUrl}/{relativePath}";
        }
    }
}
