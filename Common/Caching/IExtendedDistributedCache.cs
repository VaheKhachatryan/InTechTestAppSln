using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Caching
{
	/// <summary>
	/// Represents additional functionality on IDistrebutedCace.
	/// </summary>
	interface IExtendedDistributedCache
	{
		/// <summary>
		/// Checks is the key present in cache.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <param name="refresh">Refresh storing period or not.</param>
		bool KeyExists(string key, bool refresh = false);

		/// <summary>
		/// Checks is the key present in cache.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <param name="refresh">Refresh storing period or not.</param>
		/// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
		Task<bool> KeyExistsAsync(string key, bool refresh = false, CancellationToken token = default);

		/// <summary>
		/// Removes items by key pattern.
		/// </summary>
		/// <param name="pattern">String key pattern.</param>
		void RemoveByPattern(string pattern);

		/// <summary>
		/// Removes items by key pattern.
		/// </summary>
		/// <param name="pattern">String key pattern.</param>
		/// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
		Task RemoveByPatternAsync(string pattern, CancellationToken token = default);

		/// <summary>
		/// Removes all the keys of the database.
		/// </summary>
		void ClearAllKeys();

		/// <summary>
		/// Removes all the keys of the database.
		/// </summary>
		/// <param name="token">Optional. A <see cref="CancellationToken" /> to cancel the operation.</param>
		Task ClearAllKeysAsync(CancellationToken token = default);
		Task RefreshAbsoluteExpiryAsync(string key, TimeSpan absExpr, CancellationToken token = default(CancellationToken));
		string Instance { get; }
	}
}
