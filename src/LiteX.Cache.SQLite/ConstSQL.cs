namespace LiteX.Cache.SQLite
{
	/// <summary>
	/// Const sql.
	/// </summary>
	public static class ConstSQL
	{
		/// <summary>
		/// The setsql.
		/// </summary>
		public const string SETSQL = "\r\n                DELETE FROM [litexcache] WHERE [cachekey] = @cachekey;\r\n                INSERT INTO [litexcache]\r\n                    ([cachekey]\r\n                    ,[cachevalue]\r\n                    ,[expiration])\r\n                VALUES\r\n                    (@cachekey\r\n                    ,@cachevalue\r\n                    ,(select strftime('%s','now')) + @expiration);";

		/// <summary>
		/// The getsql.
		/// </summary>
		public const string GETSQL = "SELECT [cachevalue]\r\n                    FROM [litexcache]\r\n                    WHERE [cachekey] = @cachekey AND [expiration] > strftime('%s','now')";

		/// <summary>
		/// The getallsql.
		/// </summary>
		public const string GETALLSQL = "SELECT [cachekey],[cachevalue]\r\n                    FROM [litexcache]\r\n                    WHERE [cachekey] IN @cachekey AND [expiration] > strftime('%s','now')";

		/// <summary>
		/// The getbyprefixsql.
		/// </summary>
		public const string GETBYPREFIXSQL = "SELECT [cachekey],[cachevalue]\r\n                    FROM [litexcache]\r\n                    WHERE [cachekey] LIKE @cachekey AND [expiration] > strftime('%s','now')";

		/// <summary>
		/// The removesql.
		/// </summary>
		public const string REMOVESQL = "DELETE FROM [litexcache] WHERE [cachekey] = @cachekey ";

		/// <summary>
		/// The removebyprefixsql.
		/// </summary>
		public const string REMOVEBYPREFIXSQL = "DELETE FROM [litexcache] WHERE [cachekey] like @cachekey ";

		/// <summary>
		/// The existssql.
		/// </summary>
		public const string EXISTSSQL = "SELECT COUNT(1)\r\n                    FROM [litexcache]\r\n                    WHERE [cachekey] = @cachekey AND [expiration] > strftime('%s','now')";

		/// <summary>
		/// The countallsql.
		/// </summary>
		public const string COUNTALLSQL = "SELECT COUNT(1)\r\n            FROM [litexcache]\r\n            WHERE [expiration] > strftime('%s','now')";

		/// <summary>
		/// The countprefixsql.
		/// </summary>
		public const string COUNTPREFIXSQL = "SELECT COUNT(1)\r\n            FROM [litexcache]\r\n            WHERE [cachekey] like @cachekey AND [expiration] > strftime('%s','now')";

		/// <summary>
		/// The flushsql.
		/// </summary>
		public const string FLUSHSQL = "DELETE FROM [litexcache]";

		/// <summary>
		/// The createsql.
		/// </summary>
		public const string CREATESQL = "CREATE TABLE IF NOT EXISTS [litexcache] (\r\n                    [ID] INTEGER PRIMARY KEY\r\n                    , [cachekey] TEXT\r\n                    , [cachevalue] TEXT\r\n                    , [expiration] INTEGER)";
	}
}
