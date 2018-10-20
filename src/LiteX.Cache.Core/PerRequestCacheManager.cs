using LiteX.Log;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteX.Cache.Core
{
	/// <summary>
	/// Represents a manager for caching during an HTTP request (short term caching)
	/// </summary>
	public class PerRequestCacheManager : ICacheManager, ICacheManagerAsync, IDisposable
	{
		/// <summary>
		/// Http Context Accessor
		/// </summary>
		private readonly IHttpContextAccessor _httpContextAccessor;

		/// <summary>
		/// LiteX Logger
		/// </summary>
		protected ILiteXLogger _logger
		{
			get;
		}

		/// <summary>
		/// <see cref="T:LiteX.Cache.InMemoryCacheManager" /> 
		/// is not distributed cache.
		/// </summary>
		/// <value><c>true</c> if is distributed cache; otherwise, <c>false</c>.</value>
		public bool IsDistributedCache => false;

		/// <summary>
		/// Gets the type of the caching provider.
		/// </summary>
		/// <value>The type of the caching provider.</value>
		public CacheProviderType CacheProviderType => CacheProviderType.PerRequest;

		/// <summary>
		/// Gets a key/value collection that can be used to share data within the scope of this request 
		/// </summary>
		/// <param name="httpContextAccessor">httpContextAccessor</param>
		/// <param name="loggerFactory">Logger Factory</param>
		public PerRequestCacheManager(IHttpContextAccessor httpContextAccessor, ILiteXLoggerFactory loggerFactory = null)
		{
			_httpContextAccessor = httpContextAccessor;
			_logger = (loggerFactory?.CreateLogger(this) ?? new LiteXNullLoggerFactory().CreateLogger(this));
		}

		/// <summary>
		/// Gets or sets the value associated with the specified key.
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="key">Key of cached item</param>
		/// <returns>The cached value associated with the specified key</returns>
		public virtual T Get<T>(string key)
		{
			IDictionary<object, object> items = GetItems();
			if (items == null)
			{
				return default(T);
			}
			return (T)items[(object)key];
		}

		/// <summary>
		/// Gets or sets the value associated with the specified key.
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="key">Key of cached item</param>
		/// <returns>The cached value associated with the specified key</returns>
		public virtual Task<T> GetAsync<T>(string key)
		{
			return Task.Run(() => this.Get<T>(key));
		}

		/// <summary>
		/// Adds the specified key and object to the cache
		/// </summary>
		/// <param name="key">Key of cached item</param>
		/// <param name="data">Value for caching</param>
		/// <param name="cacheTime">Cache time in minutes</param>
		public virtual void Set(string key, object data, int cacheTime)
		{
			IDictionary<object, object> items = GetItems();
			if (items != null && data != null)
			{
				items[key] = data;
			}
		}

		/// <summary>
		/// Adds the specified key and object to the cache
		/// </summary>
		/// <param name="key">Key of cached item</param>
		/// <param name="data">Value for caching</param>
		/// <param name="cacheTime">Cache time in minutes</param>
		public virtual Task SetAsync(string key, object data, int cacheTime)
		{
			return Task.Run(delegate
			{
				Set(key, data, cacheTime);
			});
		}

		/// <summary>
		/// Gets a value indicating whether the value associated with the specified key is cached
		/// </summary>
		/// <param name="key">Key of cached item</param>
		/// <returns>True if item already is in cache; otherwise false</returns>
		public virtual bool IsSet(string key)
		{
			IDictionary<object, object> items = GetItems();
			return ((items != null) ? items[key] : null) != null;
		}

		/// <summary>
		/// Gets a value indicating whether the value associated with the specified key is cached
		/// </summary>
		/// <param name="key">Key of cached item</param>
		/// <returns>True if item already is in cache; otherwise false</returns>
		public virtual Task<bool> IsSetAsync(string key)
		{
			return Task.Run(() => IsSet(key));
		}

		/// <summary>
		/// Removes the value with the specified key from the cache
		/// </summary>
		/// <param name="key">Key of cached item</param>
		public virtual void Remove(string key)
		{
			GetItems()?.Remove(key);
		}

		/// <summary>
		/// Removes the value with the specified key from the cache
		/// </summary>
		/// <param name="key">Key of cached item</param>
		public virtual Task RemoveAsync(string key)
		{
			return Task.Run(delegate
			{
				Remove(key);
			});
		}

		/// <summary>
		/// Removes items by key pattern
		/// </summary>
		/// <param name="pattern">String key pattern</param>
		public virtual void RemoveByPattern(string pattern)
		{
			IDictionary<object, object> items = GetItems();
			if (items != null)
			{
				this.RemoveByPattern(pattern, from p in items.Keys
				select p.ToString());
			}
		}

		/// <summary>
		/// Removes items by key pattern
		/// </summary>
		/// <param name="pattern">String key pattern</param>
		public virtual Task RemoveByPatternAsync(string pattern)
		{
			return Task.Run(delegate
			{
				RemoveByPattern(pattern);
			});
		}

		/// <summary>
		/// Clear all cache data
		/// </summary>
		public virtual void Clear()
		{
			GetItems()?.Clear();
		}

		/// <summary>
		/// Clear all cache data
		/// </summary>
		public virtual Task ClearAsync()
		{
			return Task.Run(delegate
			{
				Clear();
			});
		}

		/// <summary>
		/// Dispose cache manager
		/// </summary>
		public virtual void Dispose()
		{
		}

		/// <summary>
		/// Gets a key/value collection that can be used to share data within the scope of this request 
		/// </summary>
		protected virtual IDictionary<object, object> GetItems()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			HttpContext httpContext = _httpContextAccessor.HttpContext;
			if (httpContext==null)
			{
				return null;
			}
			return httpContext.Items;
		}
	}
}
