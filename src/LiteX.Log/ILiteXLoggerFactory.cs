namespace LiteX.Log
{
	/// <summary>
	/// LiteX Logger Factory interface
	/// </summary>
	public interface ILiteXLoggerFactory
	{
		/// <summary>
		/// CreateLogger
		/// </summary>
		/// <param name="categoryName"></param>
		/// <returns></returns>
		ILiteXLogger CreateLogger(string categoryName);

		/// <summary>
		/// CreateLogger
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance"></param>
		/// <returns></returns>
		ILiteXLogger CreateLogger<T>(T instance);
	}
}
