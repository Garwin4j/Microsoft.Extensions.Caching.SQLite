using System;
using System.Threading.Tasks;

namespace LiteX.Cache.Core
{
	/// <summary>
	/// Cache manager async
	/// </summary>
	public interface ICacheManagerAsync : IDisposable
	{
		/// <summary>
		/// Gets or sets the value associated with the specified key.
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="key">Key of cached item</param>
		/// <returns>The cached value associated with the specified key</returns>
		Task<T> GetAsync<T>(string key);

		/// <summary>
		/// Adds the specified key and object to the cache
		/// </summary>
		/// <param name="key">Key of cached item</param>
		/// <param name="data">Value for caching</param>
		/// <param name="cacheTime">Cache time in minutes</param>
		Task SetAsync(string key, object data, int cacheTime);

		/// <summary>
		/// Gets a value indicating whether the value associated with the specified key is cached
		/// </summary>
		/// <param name="key">Key of cached item</param>
		/// <returns>True if item already is in cache; otherwise false</returns>
		Task<bool> IsSetAsync(string key);

		/// <summary>
		/// Removes the value with the specified key from the cache
		/// </summary>
		/// <param name="key">Key of cached item</param>
		Task RemoveAsync(string key);

		/// <summary>
		/// Removes items by key pattern
		/// </summary>
		/// <param name="pattern">String key pattern</param>
		Task RemoveByPatternAsync(string pattern);

		/// <summary>
		/// Clear all cache data
		/// </summary>
		Task ClearAsync();
	}
}
