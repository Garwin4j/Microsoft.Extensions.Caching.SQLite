namespace LiteX.Cache.Core
{
	/// <summary>
	/// Caching provider type.
	/// </summary>
	public enum CacheProviderType
	{
		/// <summary>
		/// In memory
		/// </summary>
		InMemory,
		/// <summary>
		/// Redis
		/// </summary>
		Redis,
		/// <summary>
		/// Memcached
		/// </summary>
		Memcached,
		/// <summary>
		/// SQLite
		/// </summary>
		SQLite,
		/// <summary>
		/// Per Request - HTTP request (short term caching)
		/// </summary>
		PerRequest,
		/// <summary>
		/// Other, for custom or other provider
		/// </summary>
		Other
	}
}
