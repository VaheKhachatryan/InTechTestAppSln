using System;
using System.Threading.Tasks;

namespace Common.Caching.Abstraction
{
    /// <summary>
    /// Represents a manager for caching between HTTP requests (long term caching)
    /// </summary>
    public interface IDistributedCacheManager : IDisposable
    {
        /// <summary>
        /// Get a cached item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <param name="cacheTime">Cache time in minutes; pass 0 to do not cache; pass null to use the default time</param>
        /// <returns>The cached value associated with the specified key</returns>
        T Get<T>(string key, Func<T> acquire, int? cacheTime = null);

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Async function to load item if it's not in the cache yet</param>
        /// <param name="cacheTime">Cache time in minutes; pass 0 to do not cache; pass null to use the default time</param>
        /// <returns>The cached value associated with the specified key</returns>
        Task<T> GetAsync<T>(string key, Func<Task<T>> acquire, int? cacheTime = null);

        /// <summary>
        /// Adds the specified key and object to the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="data">Value for caching</param>
        /// <param name="cacheTime">Cache time in minutes</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task SetAsync(string key, object data, int? cacheTime = null);

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached.
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="refresh">Refresh storing period or not.</param>
        /// <returns>True if item already is in cache; otherwise false</returns>
        Task<bool> KeyExistsAsync(string key, bool refresh = false);

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task RemoveAsync(string key);

        /// <summary>
        /// Removes items by key pattern
        /// </summary>
        /// <param name="prefix">String key prefix</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task RemoveByPrefixAsync(string prefix);

        /// <summary>
        /// Clear all cache data
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task ClearAsync();

        Task RefreshExpiryByKeyAsync(string key, int? cacheTime = null);
    }
}