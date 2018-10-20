using Microsoft.Data.Sqlite;
using System.IO;

namespace LiteX.Cache.SQLite
{
	/// <summary>
	/// SQLite cache Defaults
	/// </summary>
	public class SQLiteCacheDefaults
	{
		/// <summary>
		/// Default setting section name in appSettings.json
		/// </summary>
		public static string SettingsSection = "SQLiteConfig";

		/// <summary>
		/// SQLite database file name
		/// </summary>
		public static string DbFileName = "litexcaching.db";

		/// <summary>
		/// Gets or sets the file path.
		/// </summary>
		/// <value>The file path.</value>
		public static string FilePath
		{
			get;
			set;
		} = Directory.GetCurrentDirectory();


		/// <summary>
		/// Gets or sets the name of the file.
		/// </summary>
		/// <value>The name of the file.</value>
		public static string FileName
		{
			get;
			set;
		} = DbFileName;


		/// <summary>
		/// Gets or sets the open mode.
		/// </summary>
		/// <value>The open mode.</value>
		public static SqliteOpenMode OpenMode
		{
			get;
			set;
		} = 0;


		/// <summary>
		/// Gets or sets the cache mode.
		/// </summary>
		/// <value>The cache mode.</value>
		public static SqliteCacheMode CacheMode
		{
			get;
			set;
		} = 0;

	}
}
