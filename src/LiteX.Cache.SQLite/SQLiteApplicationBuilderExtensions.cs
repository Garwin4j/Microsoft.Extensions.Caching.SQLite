using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Data.Common;

namespace LiteX.Cache.SQLite
{
	/// <summary>
	/// SQLite cache application builder extensions.
	/// </summary>
	public static class SQLiteApplicationBuilderExtensions
	{
		/// <summary>
		/// Uses the SQLite cache.
		/// </summary>
		/// <returns>The SQLite cache.</returns>
		/// <param name="app">App.</param>
		public static IApplicationBuilder UseLiteXSQLiteCache(this IApplicationBuilder app)
		{
			try
			{
				SqliteConnection connection = ServiceProviderServiceExtensions.GetService<ISQLiteConnectionProvider>(app.ApplicationServices).GetConnection();
				if (((DbConnection)connection).State == ConnectionState.Closed)
				{
					((DbConnection)connection).Open();
				}
				SqlMapper.Execute((IDbConnection)connection, "CREATE TABLE IF NOT EXISTS [litexcache] (\r\n                    [ID] INTEGER PRIMARY KEY\r\n                    , [cachekey] TEXT\r\n                    , [cachevalue] TEXT\r\n                    , [expiration] INTEGER)", (object)null, (IDbTransaction)null, (int?)null, (CommandType?)null);
				return app;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return app;
			}
		}
	}
}
