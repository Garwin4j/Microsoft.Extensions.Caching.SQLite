using Microsoft.Extensions.DependencyInjection;

namespace LiteX.Cache.Core
{
	/// <summary>
	/// Per request (http) cache service collection extensions.
	/// </summary>
	public static class PerRequestCacheServiceCollectionExtensions
	{
		/// <summary>
		/// Adds LiteX per http request cache manager services
		/// Life: Scoped
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddLiteXPerRequestCache(this IServiceCollection services)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			ServiceCollectionServiceExtensions.AddScoped<ICacheManager, PerRequestCacheManager>(services);
			return services;
		}
	}
}
