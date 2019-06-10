namespace CustomCode.Core.Logging.Storage
{
    using System;
    using System.Diagnostics;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;

    /// <summary>
    /// Implementation of an <see cref="ILogMessageStore"/> that will persist <see cref="LogMessage"/>
    /// instances to the output window of the attached debugger (visual studio).
    /// </summary>
    public sealed class OutputWindowMessageStore : ILogMessageStore
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="OutputWindowMessageStore"/> type.
        /// </summary>
        /// <param name="logMessageStream">
        /// An observable stream of <see cref="LogMessage"/> instances that should be displayed
        /// in the output window.
        /// </param>
        /// <param name="formatMessage">
        /// A delegate that converts a <see cref="LogMessage"/> to a <see cref="string"/>.
        /// </param>
        public OutputWindowMessageStore(
            ILogMessageStream logMessageStream,
            Func<LogMessage, string> formatMessage)
        {
            FormatMessage = formatMessage;
            LogMessageStream = logMessageStream;
        }

        /// <summary>
        /// Gets a delegate that converts a <see cref="LogMessage"/> to a <see cref="string"/>.
        /// </summary>
        private Func<LogMessage, string> FormatMessage { get; }

        /// <summary>
        /// Gets an observable stream of <see cref="LogMessage"/> instances that should be displayed
        /// in the output window.
        /// </summary>
        private ILogMessageStream LogMessageStream { get; }

        #endregion

        #region Data

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
                var outputMessage = FormatMessage(message);
                Debug.WriteLine(outputMessage);
            }
            catch (Exception e)
            {
                try
                {
                    Debug.WriteLine($"Could not display a log message: {e.Message}");
                }
                catch
                {
                    // do nothing if Debug.WriteLine has failed
                }
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
                Debug.WriteLine(exception.Message);
            }
            catch
            {
                // do nothing if Debug.WriteLine has failed
            }
        }

        #endregion
    }
}