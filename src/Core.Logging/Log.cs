namespace CustomCode.Core.Logging
{
    using CustomCode.Core.Composition;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Default implementation of the <see cref="ILog"/> interface.
    /// </summary>
    [Export(typeof(ILog))]
    public sealed class Log : ILog
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="Log"/> type.
        /// </summary>
        /// <param name="messageCasters">
        /// A collection of <see cref="ILogMessageCaster"/> instances that will be used to
        /// transport <see cref="LogMessage"/> instances to the subscribed <see cref="ILogMessageStore"/>
        /// implementations.
        /// </param>
        public Log(IEnumerable<ILogMessageCaster> messageCasters)
        {
            MessageCasters = messageCasters;
        }

        /// <summary>
        /// Gets a collection of <see cref="ILogMessageCaster"/> instances that will be used to
        /// transport <see cref="LogMessage"/> instances to the subscribed <see cref="ILogMessageStore"/>
        /// implementations.
        /// </summary>
        private IEnumerable<ILogMessageCaster> MessageCasters { get; }

        #endregion

        #region Logic

        /// <inheritdoc />
        public void Debug(
            string message,
            [CallerMemberName] string callingMethod = "unkown",
            [CallerLineNumber] int lineNumber = -1,
            [CallerFilePath] string sourceFile = "unkown")
        {
#if DEBUG
            var logMessage = CreateMessage(message, Severity.Debug, callingMethod, lineNumber, sourceFile);
            CastMessage(logMessage);
#endif
        }

        /// <inheritdoc />
        public void Information(
            string message,
            [CallerMemberName] string callingMethod = "unknown",
            [CallerLineNumber] int lineNumber = -1,
            [CallerFilePath] string sourceFile = "unknown")
        {
            var logMessage = CreateMessage(message, Severity.Information, callingMethod, lineNumber, sourceFile);
            CastMessage(logMessage);
        }

        /// <inheritdoc />
        public void Warning(
            string message,
            [CallerMemberName] string callingMethod = "unknown",
            [CallerLineNumber] int lineNumber = -1,
            [CallerFilePath] string sourceFile = "unknown")
        {
            var logMessage = CreateMessage(message, Severity.Warning, callingMethod, lineNumber, sourceFile);
            CastMessage(logMessage);
        }

        /// <inheritdoc />
        public void Error(
            string message,
            [CallerMemberName] string callingMethod = "unknown",
            [CallerLineNumber] int lineNumber = -1,
            [CallerFilePath] string sourceFile = "unknown")
        {
            var logMessage = CreateMessage(message, Severity.Error, callingMethod, lineNumber, sourceFile);
            CastMessage(logMessage);
        }

        /// <inheritdoc />
        public void Error(
            Exception exception,
            [CallerMemberName] string callingMethod = "unknown",
            [CallerLineNumber] int lineNumber = -1,
            [CallerFilePath] string sourceFile = "unknown")
        {
            var logMessage = CreateMessage(exception, callingMethod, lineNumber, sourceFile);
            CastMessage(logMessage);
        }

        /// <summary>
        /// Create a new <see cref="LogMessage"/> dto from a given message, severity and context.
        /// </summary>
        /// <param name="message"> The message to be logged. </param>
        /// <param name="severity"> The message's <see cref="Severity"/>. </param>
        /// <param name="callingMethod"> The name of the calling method. </param>
        /// <param name="lineNumber"> The source code line number where the log method was called. </param> 
        /// <param name="sourceFile"> The name of the source code file that has made the log method call. </param>
        /// <returns> The created data transfer object. </returns>
        private LogMessage CreateMessage(string message, Severity severity,
            string callingMethod, int lineNumber, string sourceFile)
        {
            var logMessage = new LogMessage(
                callingMethod,
                Path.GetFileNameWithoutExtension(sourceFile), // assume naming convention of only one type per file with the same name
                (uint)lineNumber,
                message,
                severity,
                sourceFile,
                DateTimeOffset.Now);
            return logMessage;
        }

        /// <summary>
        /// Create a new <see cref="LogMessage"/> dto from a given message, severity and context.
        /// </summary>
        /// <param name="exception"> The exception to be logged. </param>
        /// <param name="callingMethod"> The name of the calling method. </param>
        /// <param name="lineNumber"> The source code line number where the log method was called. </param> 
        /// <param name="sourceFile"> The name of the source code file that has made the log method call. </param>
        /// <returns> The created data transfer object. </returns>
        private LogMessage CreateMessage(Exception exception,
            string callingMethod, int lineNumber, string sourceFile)
        {
            var logMessage = new LogMessage(
                callingMethod,
                Path.GetFileNameWithoutExtension(sourceFile), // assume naming convention of only one type per file with the same name
                (uint)lineNumber,
                exception.Message,
                Severity.Error,
                sourceFile,
                DateTimeOffset.Now,
                exception);
            return logMessage;
        }

        /// <summary>
        /// Cast a <see cref="LogMessage"/> to all subscribed <see cref="ILogMessageStore"/> instances.
        /// </summary>
        /// <param name="message"> The message to be cast. </param>
        private void CastMessage(LogMessage message)
        {
            try
            {
                foreach (var transportChannel in MessageCasters)
                {
                    try
                    {
                        transportChannel.Cast(message);
                    }
                    catch
                    {
                        // if a sinle transport channel is unavailable, try casting the message to all other channels
                    }
                }
            }
            catch
            {
                // do nothing if the log message could not be delivered 
            }
        }

        #endregion
    }
}