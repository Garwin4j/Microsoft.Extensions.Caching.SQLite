using System;

namespace LiteX.Cache.Core
{
	/// <summary>
	/// Represents a manager for caching between HTTP requests (long term caching)
	/// </summary>
	public interface ILiteXCacheManagerAsync : ICacheManager, ICacheManagerAsync, IDisposable
	{
	}
}
