using System;

namespace LiteX.Log
{
    /// <summary>
    /// Represents a type used to perform logging.
    /// </summary>
    public interface ILiteXLogger
    {
        /// <summary>
        /// Logs a message for the given <paramref name="logLevel" />.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="eventId">The optional even id.</param>
        /// <param name="message">The log message.</param>
        /// <param name="exception">The optional exception.</param>
        void Log(LiteXLogLevel logLevel, int eventId, object message, Exception exception);

        /// <summary>
        /// Checks if the given LogLevel is enabled.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <returns><c>True</c> if the <paramref name="logLevel" /> is enabled, <c>False</c> otherwise.</returns>
        bool IsEnabled(LiteXLogLevel logLevel);

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An <c>IDisposable</c> that ends the logical operation scope on dispose.</returns>
        IDisposable BeginScope(object state);
    }
}