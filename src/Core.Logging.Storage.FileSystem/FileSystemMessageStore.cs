namespace CustomCode.Core.Logging.Storage
{
    using System;
    using System.IO;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;

    /// <summary>
    /// Implementation of an <see cref="ILogMessageStore"/> that will persist <see cref="LogMessage"/>
    /// instances to the file system.
    /// </summary>
    public sealed class FileSystemMessageStore : ILogMessageStore
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="FileSystemMessageStore"/> type.
        /// </summary>
        /// <param name="logMessageStream">
        /// An observable stream of <see cref="LogMessage"/> instances that should be stored
        /// in the file system.
        /// </param>
        /// <param name="filePath"> The path to the log file on the harddisk. </param>
        /// <param name="formatMessageAction">
        /// A delegate that converts a <see cref="LogMessage"/> to a <see cref="string"/>.
        /// </param>
        /// <param name="headerAction"> A delegate that get the optional file header as <see cref="string"/>. </param>
        public FileSystemMessageStore(
            ILogMessageStream logMessageStream,
            string filePath,
            Func<LogMessage, string> formatMessageAction,
            Func<string>? headerAction = null)
        {
            FilePath = filePath;
            FormatMessageAction = formatMessageAction;
            HeaderAction = headerAction;
            LogMessageStream = logMessageStream;
        }

        /// <summary>
        /// Gets a delegate that converts a <see cref="LogMessage"/> to a <see cref="string"/>.
        /// </summary>
        private Func<LogMessage, string> FormatMessageAction { get; }

        /// <summary>
        /// Gets a delegate that get the optional file header as <see cref="string"/>.
        /// </summary>
        private Func<string>? HeaderAction { get; }

        /// <summary>
        /// Gets an observable stream of <see cref="LogMessage"/> instances that should be stored
        /// in the file system.
        /// </summary>
        private ILogMessageStream LogMessageStream { get; }

        #endregion

        #region Data

        /// <summary>
        /// Gets the path to the log file on the harddisk.
        /// </summary>
        private string FilePath { get; }

        /// <summary>
        /// Gets the subscription to the observed <see cref="LogMessage"/> stream.
        /// </summary>
        private IDisposable? Subscription { get; set; }

        #endregion

        #region Logic

        /// <inheritdoc />
        public void PersistMessage(LogMessage message)
        {
            try
            {
                var outputMessage = FormatMessageAction(message);
                if (HeaderAction != null && File.Exists(FilePath) == false)
                {
                    using (var writer = File.CreateText(FilePath))
                    {
                        writer.WriteLine(HeaderAction());
                        writer.WriteLine(outputMessage);
                    }
                }
                else
                {
                    using (var writer = File.AppendText(FilePath))
                    {
                        writer.WriteLine(outputMessage);
                    }
                }
            }
            catch
            {
                // do nothing if logging has failed
            }
        }

        /// <inheritdoc />
        public void Start(IScheduler? scheduler = null)
        {
            if (Subscription == null)
            {
                if (scheduler == null)
                {
                    Subscription = LogMessageStream.Subscribe(PersistMessage, OnException, Stop);
                }
                else
                {
                    Subscription = LogMessageStream.ObserveOn(scheduler).Subscribe(PersistMessage, OnException, Stop);
                }
            }
        }

        /// <inheritdoc />
        public void Stop()
        {
            if (Subscription != null)
            {
                Subscription.Dispose();
                Subscription = null;
            }
        }

        /// <summary>
        /// Event that is raised when an exception was raised from the <see cref="ILogMessageStream"/>.
        /// </summary>
        /// <param name="exception"> The (technical) exception that was raised. </param>
        private void OnException(Exception exception)
        {
            try
            {
                var message = new LogMessage(
                    nameof(ILogMessageStream.Subscribe),
                    nameof(ILogMessageStream),
                    0,
                    exception.Message,
                    Severity.Error,
                    "unknown",
                    DateTimeOffset.Now,
                    exception);
                PersistMessage(message);
            }
            catch
            {
                // do nothing if logging has failed
            }
        }

        #endregion
    }
}