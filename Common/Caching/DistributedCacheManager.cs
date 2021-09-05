using BE.Common.ComponentModel;
using BE.Common.Helpers;
using Common.Caching.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Caching
{
	/// <summary>
	/// Represents a distributed cache 
	/// </summary>
	public class DistributedCacheManager : IDistributedCacheManager
	{
		#region Fields

		private readonly IDistributedCache _distributedCache;
		private bool _disposedValue;

		#endregion

		#region Ctor

		public DistributedCacheManager(IDistributedCache distributedCache, IHttpContextAccessor httpContextAccessor)
		{
			_distributedCache = distributedCache;
		}

		#endregion

		#region Utilities

		/// <summary>
		/// Prepare cache entry options for the passed key
		/// </summary>
		/// <param name="cacheTime">Cache time in minutes</param>
		/// <returns>Cache entry options</returns>
		private static DistributedCacheEntryOptions PrepareEntryOptions(int cacheTime)
		{
			//set expiration time for the passed cache key
			var options = new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheTime)
			};

			return options;
		}

		/// <summary>
		/// Try to get the cached item
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="key">Cache key</param>
		/// <returns>
		/// A task that represents the asynchronous operation
		/// The task result contains the flag which indicate is the key exists in the cache, cached item or default value
		/// </returns>
		private async Task<T> TryGetItemAsync<T>(string key)
		{
			var json = await _distributedCache.GetStringAsync(key);

			if (string.IsNullOrEmpty(json))
			{
				return default;
			}

			var item = CoreHelper.DeserializeJson<T>(json);

			return item;
		}

		/// <summary>
		/// Try to get the cached item
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="key">Cache key</param>
		/// <returns>Flag which indicate is the key exists in the cache, cached item or default value</returns>
		private T TryGetItem<T>(string key)
		{
			var json = _distributedCache.GetString(key);

			if (string.IsNullOrEmpty(json))
			{
				return default;
			}

			var item = CoreHelper.DeserializeJson<T>(json);

			return item;
		}

		/// <summary>
		/// Add the specified key and object to the cache
		/// </summary>
		/// <param name="key">Key of cached item</param>
		/// <param name="data">Value for caching</param>
		/// <param name="cacheTime">Cache time in minutes</param>
		private void Set(string key, object data, int cacheTime)
		{
			if (cacheTime <= 0 || data == null)
			{
				return;
			}

			_distributedCache.SetString(key, CoreHelper.SerializeJson(data), PrepareEntryOptions(cacheTime));
		}

		#endregion

		#region Methods

		protected void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				_disposedValue = true;
			}
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		public async Task<T> GetAsync<T>(string key)
		{
			var item  = await TryGetItemAsync<T>(key);

			return item;
		}

		public async Task<T> GetAsync<T>(string key, Func<Task<T>> acquire, int? cacheTime = null)
		{
			cacheTime ??= DistributedCacheConfig.CacheTime;

			if (cacheTime <= 0)
			{
				return await acquire();
			}

			var item = await TryGetItemAsync<T>(key);

			var result = await acquire();

			if (result != null)
			{
				await SetAsync(key, result, cacheTime);
			}

			return result;
		}

		public async Task<T> GetAsync<T>(string key, Func<T> acquire, int? cacheTime = null)
		{
			cacheTime ??= DistributedCacheConfig.CacheTime;

			if (cacheTime <= 0)
			{
				return acquire();
			}

			var item = await TryGetItemAsync<T>(key);

			var result = acquire();

			if (result != null)
			{
				await SetAsync(key, result, cacheTime);
			}

			return result;
		}

		public T Get<T>(string key, Func<T> acquire, int? cacheTime = null)
		{
			cacheTime ??= DistributedCacheConfig.CacheTime;

			if (cacheTime <= 0)
			{
				return acquire();
			}

			var item = TryGetItem<T>(key);

			var result = acquire();

			if (result != null)
			{
				Set(key, result, cacheTime.Value);
			}

			return result;
		}

		public Task SetAsync(string key, object data, int? cacheTime = null)
		{
			if (cacheTime <= 0 || data == null)
			{
				return Task.CompletedTask;
			}

			return _distributedCache.SetStringAsync(key, CoreHelper.SerializeJson(data), PrepareEntryOptions(cacheTime ?? DistributedCacheConfig.CacheTime));
		}

		public async Task<bool> KeyExistsAsync(string key, bool refresh = false)
		{
			return await _distributedCache.KeyExistsAsync(key, refresh);
		}

		public Task RemoveAsync(string key)
		{
			return _distributedCache.RemoveAsync(key);
		}

		public Task RemoveByPrefixAsync(string prefix)
		{
			return _distributedCache.RemoveByPrefixAsync(prefix);
		}

		public Task ClearAsync()
		{
			return _distributedCache.ClearAllKeysAsync();
		}

		public async Task RefreshExpiryByKeyAsync(string key, int? cacheTime = null)
		{

			if (cacheTime <= 0)
			{
				return;
			}

			await  _distributedCache.RefreshAbsoluteExpiryAsync(key, TimeSpan.FromSeconds(cacheTime ?? DistributedCacheConfig.CacheTime));
		}
		#endregion
	}
}