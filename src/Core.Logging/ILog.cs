namespace CustomCode.Core.Logging
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Interface for the logging service that can be injected using dependency injection.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Log a message with <see cref="Severity.Debug"/> to all subscribed <see cref="ILogMessageStore"/>s.
        /// </summary>
        /// <param name="message"> The message to be logged. </param>
        /// <param name="callingMethod"> Compiler generated parameter. </param>
        /// <param name="lineNumber"> Compiler generated parameter. </param>
        /// <param name="sourceFile"> Compiler generated parameter. </param>
        /// <remarks>
        /// Note that this method will only be executed in DEBUG builds.
        /// </remarks>
        void Debug(
            string message,
            [CallerMemberName] string callingMethod = "unknown",
            [CallerLineNumber] int lineNumber = -1,
            [CallerFilePath] string sourceFile = "unknown");

        /// <summary>
        /// Log a message with <see cref="Severity.Information"/> to all subscribed <see cref="ILogMessageStore"/>s.
        /// </summary>
        /// <param name="message"> The message to be logged. </param>
        /// <param name="callingMethod"> Compiler generated parameter. </param>
        /// <param name="lineNumber"> Compiler generated parameter. </param>
        /// <param name="sourceFile"> Compiler generated parameter. </param>
        void Information(
            string message,
            [CallerMemberName] string callingMethod = "unknown",
            [CallerLineNumber] int lineNumber = -1,
            [CallerFilePath] string sourceFile = "unknown");

        /// <summary>
        /// Log a message with <see cref="Severity.Warning"/> to all subscribed <see cref="ILogMessageStore"/>s.
        /// </summary>
        /// <param name="message"> The message to be logged. </param>
        /// <param name="callingMethod"> Compiler generated parameter. </param>
        /// <param name="lineNumber"> Compiler generated parameter. </param>
        /// <param name="sourceFile"> Compiler generated parameter. </param>
        void Warning(
            string message,
            [CallerMemberName] string callingMethod = "unknown",
            [CallerLineNumber] int lineNumber = -1,
            [CallerFilePath] string sourceFile = "unknown");

        /// <summary>
        /// Log a message with <see cref="Severity.Error"/> to all subscribed <see cref="ILogMessageStore"/>s.
        /// </summary>
        /// <param name="message"> The message to be logged. </param>
        /// <param name="callingMethod"> Compiler generated parameter. </param>
        /// <param name="lineNumber"> Compiler generated parameter. </param>
        /// <param name="sourceFile"> Compiler generated parameter. </param>
        void Error(
            string message,
            [CallerMemberName] string callingMethod = "unknown",
            [CallerLineNumber] int lineNumber = -1,
            [CallerFilePath] string sourceFile = "unknown");

        /// <summary>
        /// Log a message with <see cref="Severity.Error"/> to all subscribed <see cref="ILogMessageStore"/>s.
        /// </summary>
        /// <param name="exception"> The exception to be logged. </param>
        /// <param name="callingMethod"> Compiler generated parameter. </param>
        /// <param name="lineNumber"> Compiler generated parameter. </param>
        /// <param name="sourceFile"> Compiler generated parameter. </param>
        void Error(
            Exception exception,
            [CallerMemberName] string callingMethod = "unknown",
            [CallerLineNumber] int lineNumber = -1,
            [CallerFilePath] string sourceFile = "unknown");

    }
}