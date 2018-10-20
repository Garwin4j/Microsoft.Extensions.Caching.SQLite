using Microsoft.Data.Sqlite;
using System;

namespace LiteX.Cache.SQLite
{
	/// <summary>
	/// Represents SQLite connection provider
	/// SQLite database provider
	/// </summary>
	public interface ISQLiteConnectionProvider : IDisposable
	{
		/// <summary>
		/// Gets the connection.
		/// </summary>
		/// <returns>The connection.</returns>
		SqliteConnection GetConnection();
	}
}
