namespace LiteX.Cache.SQLite
{
	/// <summary>
	/// SQLite settings
	/// </summary>
	public static class SQLiteConfiguration
	{
		/// <summary>
		/// Get the key used to store the protection key list
		/// </summary>
		public static string DataProtectionKeysName => "LiteX.DataProtectionKeys";
	}
}
