namespace CustomCode.Core.Logging
{
    using System.Reactive.Concurrency;

    /// <summary>
    /// Interface for <see cref="LogMessage"/> persistance (e.g. a file system log file).
    /// </summary>
    /// <remarks>
    /// Store implementations are subscribers to a <see cref="ILogMessageStream"/>.
    /// </remarks>
    public interface ILogMessageStore
    {
        /// <summary>
        /// Persist a reveived <see cref="LogMessage"/> (e.g. to a log file).
        /// </summary>
        /// <param name="message"> The message to be persisted. </param>
        void PersistMessage(LogMessage message);

        /// <summary>
        /// Start persisting <see cref="LogMessage"/> instances on the specified <paramref name="scheduler"/>.
        /// </summary>
        /// <param name="scheduler"> The store's <see cref="IScheduler"/>. </param>
        void Start(IScheduler? scheduler = null);

        /// <summary>
        /// Stops persisting <see cref="LogMessage"/> instances.
        /// </summary>
        void Stop();
    }
}