using System;

namespace LiteX.Log
{
	/// <summary>
	/// LiteX NullLogger
	/// </summary>
	public class LiteXNullLogger : ILiteXLogger
	{
		/// <summary>
		/// BeginScope
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public IDisposable BeginScope(object state)
		{
			return null;
		}

		/// <summary>
		/// IsEnabled
		/// </summary>
		/// <param name="logLevel"></param>
		/// <returns></returns>
		public bool IsEnabled(LiteXLogLevel logLevel)
		{
			return false;
		}

		/// <summary>
		/// Log
		/// </summary>
		/// <param name="logLevel"></param>
		/// <param name="eventId"></param>
		/// <param name="message"></param>
		/// <param name="exception"></param>
		public void Log(LiteXLogLevel logLevel, int eventId, object message, Exception exception)
		{
		}
	}
}
