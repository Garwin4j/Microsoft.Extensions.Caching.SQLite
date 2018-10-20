using LiteX.Cache.Core;
using Microsoft.Data.Sqlite;
using System.IO;

namespace LiteX.Cache.SQLite
{
	/// <summary>
	/// Startup configuration parameters
	/// SQLite cache option
	/// </summary>
	public class SQLiteConfig : LiteXCacheConfig
	{
		/// <summary>
		/// Gets or sets the file path.
		/// </summary>
		/// <value>The file path.</value>
		public string FilePath
		{
			get;
			set;
		} = Directory.GetCurrentDirectory();


		/// <summary>
		/// Gets or sets the name of the file.
		/// </summary>
		/// <value>The name of the file.</value>
		public string FileName
		{
			get;
			set;
		} = SQLiteCacheDefaults.DbFileName;


		/// <summary>
		/// Gets or sets the open mode.
		/// </summary>
		/// <value>The open mode.</value>
		public SqliteOpenMode OpenMode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the cache mode.
		/// </summary>
		/// <value>The cache mode.</value>
		public SqliteCacheMode CacheMode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the data source.
		/// </summary>
		/// <value>The data source.</value>
		public string DataSource
		{
			get
			{
				if (string.IsNullOrWhiteSpace(FilePath) && string.IsNullOrWhiteSpace(FileName))
				{
					return ":memory:";
				}
				return Path.Combine(FilePath, FileName);
			}
		}
	}
}
