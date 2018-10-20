using System;
using System.Globalization;

namespace LiteX.Log
{
	/// <summary>
	/// LiteX Logger Extensions
	/// </summary>
	public static class LiteXLoggerExtensions
	{
		/// <summary>
		/// FormatMessage
		/// </summary>
		private class FormatMessage
		{
			private readonly string _format;

			private readonly object[] _args;

			/// <summary>
			/// Ctor
			/// </summary>
			/// <param name="format"></param>
			/// <param name="args"></param>
			public FormatMessage(string format, params object[] args)
			{
				_format = format;
				_args = args;
			}

			/// <summary>
			/// ToString
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				if (_args != null && _args.Length != 0)
				{
					try
					{
						return string.Format(CultureInfo.CurrentCulture, _format, _args);
					}
					catch
					{
						return "Failed to format string";
					}
				}
				return _format;
			}
		}

		/// <summary>
		/// LogCritical
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogCritical(this ILiteXLogger logger, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Critical, 0, new FormatMessage(message, args), null);
		}

		/// <summary>
		/// LogCritical
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="eventId"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogCritical(this ILiteXLogger logger, int eventId, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Critical, eventId, new FormatMessage(message, args), null);
		}

		/// <summary>
		/// LogCritical
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="exception"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogCritical(this ILiteXLogger logger, Exception exception, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Critical, 0, new FormatMessage(message, args), exception);
		}

		/// <summary>
		/// LogCritical
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="eventId"></param>
		/// <param name="exception"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogCritical(this ILiteXLogger logger, int eventId, Exception exception, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Critical, eventId, new FormatMessage(message, args), exception);
		}

		/// <summary>
		/// LogError
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogError(this ILiteXLogger logger, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Error, 0, new FormatMessage(message, args), null);
		}

		/// <summary>
		/// LogError
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="eventId"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogError(this ILiteXLogger logger, int eventId, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Error, eventId, new FormatMessage(message, args), null);
		}

		/// <summary>
		/// LogError
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="exception"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogError(this ILiteXLogger logger, Exception exception, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Error, 0, new FormatMessage(message, args), exception);
		}

		/// <summary>
		/// LogError
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="eventId"></param>
		/// <param name="exception"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogError(this ILiteXLogger logger, int eventId, Exception exception, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Error, eventId, new FormatMessage(message, args), exception);
		}

		/// <summary>
		/// LogWarn
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogWarn(this ILiteXLogger logger, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Warning, 0, new FormatMessage(message, args), null);
		}

		/// <summary>
		/// LogWarn
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="eventId"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogWarn(this ILiteXLogger logger, int eventId, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Warning, eventId, new FormatMessage(message, args), null);
		}

		/// <summary>
		/// LogWarn
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="exception"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogWarn(this ILiteXLogger logger, Exception exception, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Warning, 0, new FormatMessage(message, args), exception);
		}

		/// <summary>
		/// LogWarn
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="eventId"></param>
		/// <param name="exception"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogWarn(this ILiteXLogger logger, int eventId, Exception exception, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Warning, eventId, new FormatMessage(message, args), exception);
		}

		/// <summary>
		/// LogInfo
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogInfo(this ILiteXLogger logger, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Information, 0, new FormatMessage(message, args), null);
		}

		/// <summary>
		/// LogInfo
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="eventId"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogInfo(this ILiteXLogger logger, int eventId, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Information, eventId, new FormatMessage(message, args), null);
		}

		/// <summary>
		/// LogInfo
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="exception"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogInfo(this ILiteXLogger logger, Exception exception, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Information, 0, new FormatMessage(message, args), exception);
		}

		/// <summary>
		/// LogInfo
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="eventId"></param>
		/// <param name="exception"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogInfo(this ILiteXLogger logger, int eventId, Exception exception, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Information, eventId, new FormatMessage(message, args), exception);
		}

		/// <summary>
		/// LogDebug
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogDebug(this ILiteXLogger logger, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Debug, 0, new FormatMessage(message, args), null);
		}

		/// <summary>
		/// LogDebug
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="eventId"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogDebug(this ILiteXLogger logger, int eventId, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Debug, eventId, new FormatMessage(message, args), null);
		}

		/// <summary>
		/// LogDebug
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="exception"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogDebug(this ILiteXLogger logger, Exception exception, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Debug, 0, new FormatMessage(message, args), exception);
		}

		/// <summary>
		/// LogDebug
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="eventId"></param>
		/// <param name="exception"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogDebug(this ILiteXLogger logger, int eventId, Exception exception, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Debug, eventId, new FormatMessage(message, args), exception);
		}

		/// <summary>
		/// LogTrace
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogTrace(this ILiteXLogger logger, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Trace, 0, new FormatMessage(message, args), null);
		}

		/// <summary>
		/// LogTrace
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="eventId"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogTrace(this ILiteXLogger logger, int eventId, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Trace, eventId, new FormatMessage(message, args), null);
		}

		/// <summary>
		/// LogTrace
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="exception"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogTrace(this ILiteXLogger logger, Exception exception, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Trace, 0, new FormatMessage(message, args), exception);
		}

		/// <summary>
		/// LogTrace
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="eventId"></param>
		/// <param name="exception"></param>
		/// <param name="message"></param>
		/// <param name="args"></param>
		public static void LogTrace(this ILiteXLogger logger, int eventId, Exception exception, string message, params object[] args)
		{
			if (logger == null)
			{
				throw new ArgumentNullException("logger");
			}
			logger.Log(LiteXLogLevel.Trace, eventId, new FormatMessage(message, args), exception);
		}
	}
}
