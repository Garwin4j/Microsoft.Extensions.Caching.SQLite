using System;

namespace LiteX.Cache.Core
{
	/// <summary>
	/// Cache manager
	/// </summary>
	public interface ICacheManager : ICacheManagerAsync, IDisposable
	{
		/// <summary>
		/// Gets a value indicating whether this <see cref="T:LiteX.Cache.Core.ICacheManager" /> is distributed cache.
		/// </summary>
		/// <value><c>true</c> if is distributed cache; otherwise, <c>false</c>.</value>
		bool IsDistributedCache
		{
			get;
		}

		/// <summary>
		/// Gets the type of the cache provider.
		/// </summary>
		/// <value>The type of the cache provider.</value>
		CacheProviderType CacheProviderType
		{
			get;
		}

		/// <summary>
		/// Gets or sets the value associated with the specified key.
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="key">Key of cached item</param>
		/// <returns>The cached value associated with the specified key</returns>
		T Get<T>(string key);

		/// <summary>
		/// Adds the specified key and object to the cache
		/// </summary>
		/// <param name="key">Key of cached item</param>
		/// <param name="data">Value for caching</param>
		/// <param name="cacheTime">Cache time in minutes</param>
		void Set(string key, object data, int cacheTime);

		/// <summary>
		/// Gets a value indicating whether the value associated with the specified key is cached
		/// </summary>
		/// <param name="key">Key of cached item</param>
		/// <returns>True if item already is in cache; otherwise false</returns>
		bool IsSet(string key);

		/// <summary>
		/// Removes the value with the specified key from the cache
		/// </summary>
		/// <param name="key">Key of cached item</param>
		void Remove(string key);

		/// <summary>
		/// Removes items by key pattern
		/// </summary>
		/// <param name="pattern">String key pattern</param>
		void RemoveByPattern(string pattern);

		/// <summary>
		/// Clear all cache data
		/// </summary>
		void Clear();
	}
}
