using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Caching
{
	/// <summary>
	/// Extension methods for removing data in an <see cref="IDistributedCache" />.
	/// </summary>
	public static class DistributedCacheExtensions
	{
		/// <summary>
		/// Checks is the key present in cache.
		/// </summary>
		/// <param name="cache">The cache in which to store the data.</param>
		/// <param name="key">The key to check.</param>
		/// <param name="refresh">Refresh storing period or not.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null or empty.</exception>
		/// <exception cref="Exception">Thrown when <paramref name="cache"/> is not implement <see cref="IExtendedDistributedCache"/>.</exception>
		public static bool KeyExists(this IDistributedCache cache, string key, bool refresh = false)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (cache is not IExtendedDistributedCache cacheExtended)
			{
				throw new Exception($"Selected distributed cache not implements {nameof(IExtendedDistributedCache)}.");
			}

			return cacheExtended.KeyExists(((IExtendedDistributedCache)cache).Instance + key, refresh);
		}

		/// <summary>
		/// Checks is the key present in cache.
		/// </summary>
		/// <param name="cache">The cache in which to store the data.</param>
		/// <param name="key">The key to check.</param>
		/// <param name="refresh">Refresh storing period or not.</param>
		/// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null or empty.</exception>
		/// <exception cref="Exception">Thrown when <paramref name="cache"/> is not implement <see cref="IExtendedDistributedCache"/>.</exception>
		public static Task<bool> KeyExistsAsync(this IDistributedCache cache, string key, bool refresh = false, CancellationToken token = default)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentNullException(nameof(key));
			}

			if (cache is not IExtendedDistributedCache cacheExtended)
			{
				throw new Exception($"Selected distributed cache not implements {nameof(IExtendedDistributedCache)}.");
			}

			return cacheExtended.KeyExistsAsync(((IExtendedDistributedCache)cache).Instance + key, refresh, token);
		}

		/// <summary>
		/// Removes items by key pattern.
		/// </summary>
		/// <param name="cache">The cache in which to store the data.</param>
		/// <param name="prefix">The prefix to remove the data.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="prefix"/> is null or empty.</exception>
		/// <exception cref="Exception">Thrown when <paramref name="cache"/> is not implement <see cref="IExtendedDistributedCache"/>.</exception>
		public static void RemoveByPrefix(this IDistributedCache cache, string prefix)
		{
			if (string.IsNullOrWhiteSpace(prefix))
			{
				throw new ArgumentNullException(nameof(prefix));
			}

			if (cache is not IExtendedDistributedCache cacheExtended)
			{
				throw new Exception($"Selected distributed cache not implements {nameof(IExtendedDistributedCache)}.");
			}

			cacheExtended.RemoveByPattern(prefix);
		}

		/// <summary>
		/// Removes items by key pattern.
		/// </summary>
		/// <param name="cache">The cache in which to store the data.</param>
		/// <param name="prefix">The prefix to remove the data.</param>
		/// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="prefix"/> is null or empty.</exception>
		/// <exception cref="Exception">Thrown when <paramref name="cache"/> is not implement <see cref="IExtendedDistributedCache"/>.</exception>
		public static Task RemoveByPrefixAsync(this IDistributedCache cache, string prefix, CancellationToken token = default)
		{
			if (string.IsNullOrWhiteSpace(prefix))
			{
				throw new ArgumentNullException(nameof(prefix));
			}

			if (cache is not IExtendedDistributedCache cacheExtended)
			{
				throw new Exception($"Selected distributed cache not implements {nameof(IExtendedDistributedCache)}.");
			}

			return cacheExtended.RemoveByPatternAsync(prefix, token);
		}

		/// <summary>
		/// Removes all the keys of the database.
		/// </summary>
		/// <param name="cache">The cache in which to store the data.</param>
		/// <exception cref="Exception">Thrown when <paramref name="cache"/> is not implement <see cref="IExtendedDistributedCache"/>.</exception>
		public static void ClearAllKeys(this IDistributedCache cache)
		{
			if (cache is not IExtendedDistributedCache cacheExtended)
			{
				throw new Exception($"Selected distributed cache not implements {nameof(IExtendedDistributedCache)}.");
			}

			cacheExtended.ClearAllKeys();
		}

		/// <summary>
		/// Removes all the keys of the database.
		/// </summary>
		/// <param name="cache">The cache in which to store the data.</param>
		/// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
		/// <exception cref="Exception">Thrown when <paramref name="cache"/> is not implement <see cref="IExtendedDistributedCache"/>.</exception>
		public static Task ClearAllKeysAsync(this IDistributedCache cache, CancellationToken token = default)
		{
			if (cache is not IExtendedDistributedCache cacheExtended)
			{
				throw new Exception($"Selected distributed cache not implements {nameof(IExtendedDistributedCache)}.");
			}

			return cacheExtended.ClearAllKeysAsync(token);
		}

		public static Task RefreshAbsoluteExpiryAsync(this IDistributedCache cache, string key, TimeSpan absExpr, CancellationToken token = default(CancellationToken))
		{
			if (cache is not IExtendedDistributedCache cacheExtended)
			{
				throw new Exception($"Selected distributed cache not implements {nameof(IExtendedDistributedCache)}.");
			}

			return cacheExtended.RefreshAbsoluteExpiryAsync(key, absExpr, token);
		}
	}
}
