using Microsoft.Data.Sqlite;
using System;
using System.ComponentModel;

namespace LiteX.Cache.SQLite
{
	/// <summary>
	/// Represents SQLite connection provider implementation
	/// </summary>
	public class SQLiteConnectionProvider : ISQLiteConnectionProvider, IDisposable
	{
		private readonly SQLiteConfig _config;

		/// <summary>
		/// The conn.
		/// </summary>
		private static SqliteConnection _connection;

		private readonly object _lock = new object();

		/// <summary>
		/// Initializes a new instance of the <see cref="T:LiteX.Cache.SQLite.SQLiteConnectionProvider" /> class.
		/// Ctor
		/// </summary>
		/// <param name="config">Config</param>
		public SQLiteConnectionProvider(SQLiteConfig config)
		{
			_config = config;
		}

		/// <summary>
		/// Gets the connection.
		/// </summary>
		/// <returns>The connection.</returns>
		public SqliteConnection GetConnection()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			if (_connection == null)
			{
				SqliteConnectionStringBuilder val = new SqliteConnectionStringBuilder();
				val.DataSource=(_config.DataSource);
				val.Mode=(_config.OpenMode);
				val.Cache=(_config.CacheMode);
				_connection = new SqliteConnection(((object)val).ToString());
			}
			return _connection;
		}

		/// <summary>
		/// Release all resources associated with this object
		/// </summary>
		public void Dispose()
		{
			((Component)_connection)?.Dispose();
		}
	}
}
