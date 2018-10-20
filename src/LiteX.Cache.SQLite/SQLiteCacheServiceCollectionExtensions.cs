using LiteX.Cache.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LiteX.Cache.SQLite
{
	/// <summary>
	/// SQLite cache service collection extensions.
	/// </summary>
	public static class SQLiteCacheServiceCollectionExtensions
	{
		/// <summary>
		/// Adds LiteX SQLite Cache manager services
		/// </summary>
		/// <param name="services"></param>
		/// <param name="options">Option setup.</param>
		/// <returns></returns>
		public static IServiceCollection AddLiteXSQLiteCache(this IServiceCollection services, Action<SQLiteConfig> options)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			SQLiteConfig sQLiteConfig = new SQLiteConfig();
			options(sQLiteConfig);
			ServiceCollectionServiceExtensions.AddSingleton<SQLiteConfig>(services, sQLiteConfig);
			services.AddLiteXPerRequestCache();
			ServiceCollectionServiceExtensions.AddSingleton<ISQLiteConnectionProvider, SQLiteConnectionProvider>(services);
			ServiceCollectionServiceExtensions.AddScoped<ILiteXCacheManager, SQLiteCacheManager>(services);
			ServiceCollectionServiceExtensions.AddScoped<ILiteXCacheManagerAsync, SQLiteCacheManager>(services);
			return services;
		}

		/// <summary>
		/// Adds LiteX SQLite Cache manager services
		/// </summary>
		/// <param name="services"></param>
		/// <param name="config">SQLite configuration settings, default: use 'SQLiteConfig' from appsettings.json</param>
		/// <returns></returns>
		public static IServiceCollection AddLiteXSQLiteCache(this IServiceCollection services, SQLiteConfig config = null)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			if (config == null)
			{
				IConfiguration requiredService = ServiceProviderServiceExtensions.GetRequiredService<IConfiguration>((IServiceProvider)ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services));
				IConfigurationSection section = requiredService.GetSection(SQLiteCacheDefaults.SettingsSection);
				config = ((section !=null) ? ConfigurationBinder.Get<SQLiteConfig>(section) : null);
				if (config == null)
				{
					config = new SQLiteConfig
					{
						FileName = SQLiteCacheDefaults.FileName,
						FilePath = SQLiteCacheDefaults.FilePath,
						OpenMode = 0,
						CacheMode = 0,
						EnableLogging = LiteXCacheDefaults.EnableLogging
					};
				}
				config.FilePath = ((!string.IsNullOrWhiteSpace(config.FilePath)) ? config.FilePath : SQLiteCacheDefaults.FilePath);
			}
			return services.AddLiteXSQLiteCache(delegate(SQLiteConfig option)
			{
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				option.FileName = config.FileName;
				option.FilePath = config.FilePath;
				option.OpenMode = config.OpenMode;
				option.CacheMode = config.CacheMode;
			});
		}
	}
}
