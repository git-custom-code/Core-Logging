namespace CustomCode.Core.Logging
{
    using System;

    /// <summary>
    /// Data transfer object that contains the data to be logged.
    /// </summary>
    public sealed class LogMessage
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="LogMessage"/> type.
        /// </summary>
        /// <param name="callingMethod">
        /// The name of the method that has made the call to one of the methods of the <see cref="ILog"/> service.
        /// </param>
        /// <param name="callingType">
        /// The name of the type that has made the call to one of the methods of the <see cref="ILog"/> service.
        /// </param>
        /// <param name="lineNumber">
        /// The line number inside the source code file where the call to one of the methods of the
        /// <see cref="ILog"/> service was made.
        /// </param>
        /// <param name="message"> The message to be logged. </param>
        /// <param name="severity"> The message's severity. </param>
        /// <param name="sourceFile">
        /// The name of the source code file that has made the call to one of the methods
        /// of the <see cref="ILog"/> service.
        /// </param>
        /// <param name="timestamp">
        /// The timestamp (in utc) when the call to one of the methods of the <see cref="ILog"/> service was made.
        /// </param>
        /// <param name="exception"> The (optional) exception to be logged. </param>
        public LogMessage(
            string callingMethod,
            string callingType,
            uint lineNumber,
            string message,
            Severity severity,
            string sourceFile,
            DateTimeOffset? timestamp = null,
            Exception? exception = null)
        {
            CallingMethod = callingMethod;
            CallingType = callingType;
            Exception = exception;
            LineNumber = lineNumber;
            Message = message;
            Severity = severity;
            SourceFile = sourceFile;
            Timestamp = timestamp ?? DateTimeOffset.Now;
        }

        #endregion

        #region Data

        /// <summary>
        /// Gets the name of the method that has made the call to one of the methods
        /// of the <see cref="ILog"/> service.
        /// </summary>
        public string CallingMethod { get; }

        /// <summary>
        /// Gets the name of the type that has made the call to one of the methods
        /// of the <see cref="ILog"/> service.
        /// </summary>
        public string CallingType { get; }

        /// <summary>
        /// Gets the (optional) exception to be logged.
        /// </summary>
        public Exception? Exception { get; }

        /// <summary>
        /// Gets the line number inside the source code file where the call to one of the methods of the
        /// <see cref="ILog"/> service was made.
        /// </summary>
        public uint LineNumber { get; }

        /// <summary>
        /// Gets the message to be logged.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the <see cref="Message"/>'s severity.
        /// </summary>
        public Severity Severity { get; }

        /// <summary>
        /// Gets the name of the source code file that has made the call to one of the methods
        /// of the <see cref="ILog"/> service.
        /// </summary>
        public string SourceFile { get; }

        /// <summary>
        /// Gets the timestamp (in utc) when the call to one of the methods of the <see cref="ILog"/> service was made.
        /// </summary>
        public DateTimeOffset Timestamp { get; }

        #endregion
    }
}