using System;

namespace Queenwood.Core.Services.CacheService
{
    public interface ICacheService
    {
        T Get<T>(string keyName, Func<T> creator, int expiryMins);
        void Remove(string keyName);
        void RemoveAll();
    }
}
