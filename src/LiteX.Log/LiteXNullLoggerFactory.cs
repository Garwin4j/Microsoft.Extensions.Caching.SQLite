namespace LiteX.Log
{
	/// <summary>
	/// LiteX Null Logger Factory
	/// </summary>
	public class LiteXNullLoggerFactory : ILiteXLoggerFactory
	{
		/// <summary>
		/// CreateLogger
		/// </summary>
		/// <param name="categoryName"></param>
		/// <returns></returns>
		public ILiteXLogger CreateLogger(string categoryName)
		{
			return new LiteXNullLogger();
		}

		/// <summary>
		/// CreateLogger
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance"></param>
		/// <returns></returns>
		public ILiteXLogger CreateLogger<T>(T instance)
		{
			return new LiteXNullLogger();
		}
	}
}
