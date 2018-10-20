using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LiteX.Cache.Core
{
	/// <summary>
	/// Extensions of ICacheManager
	/// </summary>
	public static class CacheExtensions
	{
		/// <summary>
		/// Get a cached item. If it's not in the cache yet, then load and cache it
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="cacheManager">Cache manager</param>
		/// <param name="key">Cache key</param>
		/// <param name="acquire">Function to load item if it's not in the cache yet</param>
		/// <returns>Cached item</returns>
		public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
		{
			return cacheManager.Get(key, LiteXCacheDefaults.CacheTime, acquire);
		}

		/// <summary>
		/// Get a cached item. If it's not in the cache yet, then load and cache it
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="cacheManager">Cache manager</param>
		/// <param name="key">Cache key</param>
		/// <param name="acquire">Function to load item if it's not in the cache yet</param>
		/// <returns>Cached item</returns>
		public static async Task<T> GetAsync<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
		{
			return await CacheExtensions.GetAsync<T>(cacheManager, key, LiteXCacheDefaults.CacheTime, acquire);
		}

		/// <summary>
		/// Get a cached item. If it's not in the cache yet, then load and cache it
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="cacheManager">Cache manager</param>
		/// <param name="key">Cache key</param>
		/// <param name="cacheTime">Cache time in minutes (0 - do not cache)</param>
		/// <param name="acquire">Function to load item if it's not in the cache yet</param>
		/// <returns>Cached item</returns>
		public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
		{
			if (cacheManager.IsSet(key))
			{
				return cacheManager.Get<T>(key);
			}
			T val = acquire();
			if (cacheTime > 0)
			{
				cacheManager.Set(key, val, cacheTime);
			}
			return val;
		}

		/// <summary>
		/// Get a cached item. If it's not in the cache yet, then load and cache it
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="cacheManager">Cache manager</param>
		/// <param name="key">Cache key</param>
		/// <param name="cacheTime">Cache time in minutes (0 - do not cache)</param>
		/// <param name="acquire">Function to load item if it's not in the cache yet</param>
		/// <returns>Cached item</returns>
		public static async Task<T> GetAsync<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
		{
			if (await cacheManager.IsSetAsync(key))
			{
				return await((ICacheManagerAsync)cacheManager).GetAsync<T>(key);
			}
			T result = acquire();
			if (cacheTime > 0)
			{
				await cacheManager.SetAsync(key, result, cacheTime);
			}
			return result;
		}

		/// <summary>
		/// Removes items by pattern
		/// </summary>
		/// <param name="cacheManager">Cache manager</param>
		/// <param name="pattern">Pattern</param>
		/// <param name="keys">All keys in the cache</param>
		public static void RemoveByPattern(this ICacheManager cacheManager, string pattern, IEnumerable<string> keys)
		{
			Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
			(from key in keys
			where regex.IsMatch(key)
			select key).ToList().ForEach(cacheManager.Remove);
		}
	}
}
