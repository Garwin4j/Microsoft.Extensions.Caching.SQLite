using Dapper;
using LiteX.Cache.Core;
using LiteX.Log;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LiteX.Cache.SQLite
{
	/// <summary>
	/// SQLite cache manager for caching in SQLite store
	/// </summary>
	public class SQLiteCacheManager : ILiteXCacheManager, ILiteXCacheManagerAsync, ICacheManager, ICacheManagerAsync, IDisposable
	{
		/// <summary>
		/// Per Request Cache Manager
		/// </summary>
		private readonly ICacheManager _perRequestCacheManager;

		/// <summary>
		/// Database provider wrapper
		/// </summary>
		private readonly ISQLiteConnectionProvider _connectionWrapper;

		/// <summary>
		/// Config options.
		/// </summary>
		private readonly SQLiteConfig _config;

		/// <summary>
		/// Sqlite Connection (cache).
		/// </summary>
		private readonly SqliteConnection _connection;

		/// <summary>
		/// LiteX Logger
		/// </summary>
		protected ILiteXLogger _logger
		{
			get;
		}

		/// <summary>
		/// <see cref="T:LiteX.Cache.SQLite.SQLiteCacheManager" /> 
		/// is not distributed cache.
		/// </summary>
		/// <value><c>true</c> if is distributed cache; otherwise, <c>false</c>.</value>
		public bool IsDistributedCache => false;

		/// <summary>
		/// Gets the type of the caching provider.
		/// </summary>
		/// <value>The type of the caching provider.</value>
		public CacheProviderType CacheProviderType => CacheProviderType.SQLite;

		/// <summary>
		/// Ctor
		/// Initializes a new instance of the <see cref="T:LiteX.Cache.SQLite.SQLiteCachingProvider" /> class.
		/// </summary>
		/// <param name="perRequestCacheManager">per request cacheManager</param>
		/// <param name="connectionWrapper">Connection wrapper</param>
		/// <param name="config">Config options</param>
		/// <param name="loggerFactory">Logger Factory</param>
		public SQLiteCacheManager(ICacheManager perRequestCacheManager, ISQLiteConnectionProvider connectionWrapper, SQLiteConfig config, ILiteXLoggerFactory loggerFactory = null)
		{
			if (string.IsNullOrEmpty(config.FileName))
			{
				throw new Exception("FileName is empty");
			}
			if (string.IsNullOrEmpty(config.FilePath))
			{
				throw new Exception("FilePath is empty");
			}
			_config = config;
			_perRequestCacheManager = perRequestCacheManager;
			_connectionWrapper = connectionWrapper;
			_connection = _connectionWrapper.GetConnection();
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
			if (_config.EnableLogging)
			{
				_logger?.LogInfo($"Cache Get : cachekey = {key}", Array.Empty<object>());
			}
			if (_perRequestCacheManager.IsSet(key))
			{
				return _perRequestCacheManager.Get<T>(key);
			}
			string text = SqlMapper.Query<string>((IDbConnection)_connection, "SELECT [cachevalue]\r\n                    FROM [litexcache]\r\n                    WHERE [cachekey] = @cachekey AND [expiration] > strftime('%s','now')", (object)new
			{
				cachekey = key
			}, (IDbTransaction)null, true, (int?)null, (CommandType?)null).FirstOrDefault();
			if (!string.IsNullOrWhiteSpace(text))
			{
				if (_config.EnableLogging)
				{
					_logger?.LogInfo($"Cache Hit : cachekey = {key}", Array.Empty<object>());
				}
				T val = JsonConvert.DeserializeObject<T>(text);
				if (val == null)
				{
					return default(T);
				}
				_perRequestCacheManager.Set(key, val, 0);
				return val;
			}
			if (_config.EnableLogging)
			{
				_logger?.LogInfo($"Cache Missed : cachekey = {key}", Array.Empty<object>());
			}
			return default(T);
		}

		/// <summary>
		/// Gets or sets the value associated with the specified key.
		/// </summary>
		/// <typeparam name="T">Type of cached item</typeparam>
		/// <param name="key">Key of cached item</param>
		/// <returns>The cached value associated with the specified key</returns>
		public virtual async Task<T> GetAsync<T>(string key)
		{
			if (_config.EnableLogging)
			{
				_logger?.LogInfo($"Cache Get : cachekey = {key}", Array.Empty<object>());
			}
			if (_perRequestCacheManager.IsSet(key))
			{
				return await((ICacheManagerAsync)_perRequestCacheManager).GetAsync<T>(key);
			}
			string text = Enumerable.FirstOrDefault<string>(await SqlMapper.QueryAsync<string>((IDbConnection)_connection, "SELECT [cachevalue]\r\n                    FROM [litexcache]\r\n                    WHERE [cachekey] = @cachekey AND [expiration] > strftime('%s','now')", (object)new
			{
				cachekey = key
			}, (IDbTransaction)null, (int?)null, (CommandType?)null));
			if (!string.IsNullOrWhiteSpace(text))
			{
				if (_config.EnableLogging)
				{
					_logger?.LogInfo($"Cache Hit : cachekey = {key}", Array.Empty<object>());
				}
				T val = JsonConvert.DeserializeObject<T>(text);
				if (val == null)
				{
					return default(T);
				}
				_perRequestCacheManager.Set(key, val, 0);
				return val;
			}
			if (_config.EnableLogging)
			{
				_logger?.LogInfo($"Cache Missed : cachekey = {key}", Array.Empty<object>());
			}
			return default(T);
		}

		/// <summary>
		/// Adds the specified key and object to the cache
		/// </summary>
		/// <param name="key">Key of cached item</param>
		/// <param name="data">Value for caching</param>
		/// <param name="cacheTime">Cache time in minutes</param>
		public virtual void Set(string key, object data, int cacheTime)
		{
			if (data != null)
			{
				if (_config.EnableLogging)
				{
					_logger?.LogInfo($"Cache Set : cachekey = {key}", Array.Empty<object>());
				}
				TimeSpan timeSpan = TimeSpan.FromMinutes((double)cacheTime);
				string cachevalue = JsonConvert.SerializeObject(data);
				SqlMapper.Execute((IDbConnection)_connection, "\r\n                DELETE FROM [litexcache] WHERE [cachekey] = @cachekey;\r\n                INSERT INTO [litexcache]\r\n                    ([cachekey]\r\n                    ,[cachevalue]\r\n                    ,[expiration])\r\n                VALUES\r\n                    (@cachekey\r\n                    ,@cachevalue\r\n                    ,(select strftime('%s','now')) + @expiration);", (object)new
				{
					cachekey = key,
					cachevalue = cachevalue,
					expiration = timeSpan.Ticks / 10000000
				}, (IDbTransaction)null, (int?)null, (CommandType?)null);
			}
		}

		/// <summary>
		/// Adds the specified key and object to the cache
		/// </summary>
		/// <param name="key">Key of cached item</param>
		/// <param name="data">Value for caching</param>
		/// <param name="cacheTime">Cache time in minutes</param>
		public virtual async Task SetAsync(string key, object data, int cacheTime)
		{
			if (data != null)
			{
				if (_config.EnableLogging)
				{
					_logger?.LogInfo($"Cache Set : cachekey = {key}", Array.Empty<object>());
				}
				TimeSpan timeSpan = TimeSpan.FromMinutes((double)cacheTime);
				string cachevalue = JsonConvert.SerializeObject(data);
				await SqlMapper.ExecuteAsync((IDbConnection)_connection, "\r\n                DELETE FROM [litexcache] WHERE [cachekey] = @cachekey;\r\n                INSERT INTO [litexcache]\r\n                    ([cachekey]\r\n                    ,[cachevalue]\r\n                    ,[expiration])\r\n                VALUES\r\n                    (@cachekey\r\n                    ,@cachevalue\r\n                    ,(select strftime('%s','now')) + @expiration);", (object)new
				{
					cachekey = key,
					cachevalue = cachevalue,
					expiration = timeSpan.Ticks / 10000000
				}, (IDbTransaction)null, (int?)null, (CommandType?)null);
			}
		}

		/// <summary>
		/// Gets a value indicating whether the value associated with the specified key is cached
		/// </summary>
		/// <param name="key">Key of cached item</param>
		/// <returns>True if item already is in cache; otherwise false</returns>
		public virtual bool IsSet(string key)
		{
			if (_perRequestCacheManager.IsSet(key))
			{
				return true;
			}
			return SqlMapper.ExecuteScalar<int>((IDbConnection)_connection, "SELECT COUNT(1)\r\n                    FROM [litexcache]\r\n                    WHERE [cachekey] = @cachekey AND [expiration] > strftime('%s','now')", (object)new
			{
				cachekey = key
			}, (IDbTransaction)null, (int?)null, (CommandType?)null) == 1;
		}

		/// <summary>
		/// Gets a value indicating whether the value associated with the specified key is cached
		/// </summary>
		/// <param name="key">Key of cached item</param>
		/// <returns>True if item already is in cache; otherwise false</returns>
		public virtual async Task<bool> IsSetAsync(string key)
		{
			if (_perRequestCacheManager.IsSet(key))
			{
				return true;
			}
			return await SqlMapper.ExecuteScalarAsync<int>((IDbConnection)_connection, "SELECT COUNT(1)\r\n                    FROM [litexcache]\r\n                    WHERE [cachekey] = @cachekey AND [expiration] > strftime('%s','now')", (object)new
			{
				cachekey = key
			}, (IDbTransaction)null, (int?)null, (CommandType?)null) == 1;
		}

		/// <summary>
		/// Removes the value with the specified key from the cache
		/// </summary>
		/// <param name="key">Key of cached item</param>
		public virtual void Remove(string key)
		{
			if (_config.EnableLogging)
			{
				_logger?.LogInfo($"Cache Remove : cachekey = {key}", Array.Empty<object>());
			}
			SqlMapper.Execute((IDbConnection)_connection, "DELETE FROM [litexcache] WHERE [cachekey] = @cachekey ", (object)new
			{
				cachekey = key
			}, (IDbTransaction)null, (int?)null, (CommandType?)null);
			_perRequestCacheManager.Remove(key);
		}

		/// <summary>
		/// Removes the value with the specified key from the cache
		/// </summary>
		/// <param name="key">Key of cached item</param>
		public virtual async Task RemoveAsync(string key)
		{
			if (_config.EnableLogging)
			{
				_logger?.LogInfo($"Cache Remove : cachekey = {key}", Array.Empty<object>());
			}
			await SqlMapper.ExecuteAsync((IDbConnection)_connection, "DELETE FROM [litexcache] WHERE [cachekey] = @cachekey ", (object)new
			{
				cachekey = key
			}, (IDbTransaction)null, (int?)null, (CommandType?)null);
			_perRequestCacheManager.Remove(key);
		}

		/// <summary>
		/// Removes items by key pattern
		/// </summary>
		/// <param name="pattern">String key pattern</param>
		public virtual async void RemoveByPattern(string pattern)
		{
			await RemoveByPatternAsync(pattern);
		}

        ///// <summary>
        ///// Removes items by key pattern
        ///// NOT IMPLEMENTED
        ///// </summary>
        ///// <param name="pattern">String key pattern</param>
        //[AsyncStateMachine(typeof(_003CRemoveByPatternAsync_003Ed__21))]
        //public virtual Task RemoveByPatternAsync(string pattern)
        //{
        //	_003CRemoveByPatternAsync_003Ed__21 stateMachine = default(_003CRemoveByPatternAsync_003Ed__21);
        //	stateMachine._003C_003E4__this = this;
        //	stateMachine.pattern = pattern;
        //	stateMachine._003C_003Et__builder = AsyncTaskMethodBuilder.Create();
        //	stateMachine._003C_003E1__state = -1;
        //	AsyncTaskMethodBuilder _003C_003Et__builder = stateMachine._003C_003Et__builder;
        //	_003C_003Et__builder.Start(ref stateMachine);
        //	return stateMachine._003C_003Et__builder.Task;
        //}


        public virtual async Task RemoveByPatternAsync(string key)
        {
            if (_config.EnableLogging)
            {
                _logger?.LogInfo($"Cache Remove : cachekey = {key}", Array.Empty<object>());
            }
            await SqlMapper.ExecuteAsync((IDbConnection)_connection, "DELETE FROM [litexcache] WHERE [cachekey] like @cachekey ", (object)new
            {
                cachekey = key
            }, (IDbTransaction)null, (int?)null, (CommandType?)null);
            _perRequestCacheManager.RemoveByPattern(key);
        }


        /// <summary>
        /// Clear all cache data
        /// </summary>
        public virtual void Clear()
		{
			if (_config.EnableLogging)
			{
				_logger?.LogInfo("Cache Clear", Array.Empty<object>());
			}
			SqlMapper.Execute((IDbConnection)_connection, "DELETE FROM [litexcache]", (object)null, (IDbTransaction)null, (int?)null, (CommandType?)null);
		}

		/// <summary>
		/// Clear all cache data
		/// </summary>
		public virtual async Task ClearAsync()
		{
			_perRequestCacheManager.Clear();
			await SqlMapper.ExecuteAsync((IDbConnection)_connection, "DELETE FROM [litexcache]", (object)null, (IDbTransaction)null, (int?)null, (CommandType?)null);
		}

		/// <summary>
		/// Dispose cache manager
		/// </summary>
		public virtual void Dispose()
		{
			if (_connectionWrapper != null)
			{
				_connectionWrapper.Dispose();
			}
		}
	}
}
