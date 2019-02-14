using System;
using System.Threading.Tasks;

namespace Queenwood.Core.Services.CacheService
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string keyName, Func<T> creator, int expiryMins);
        T Get<T>(string keyName, Func<T> creator, int expiryMins);
        void Remove(string keyName);
        void RemoveAll();
    }
}
