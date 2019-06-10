namespace CustomCode.Core.Logging.Transport
{
    using System;
    using System.Collections.Concurrent;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;

    /// <summary>
    /// In-memory/in-process implementation of the <see cref="ILogMessageStream"/> and
    /// <see cref="ILogMessageCaster"/> interfaces that enables transport of <see cref="LogMessage"/>
    /// instances within the same process.
    /// </summary>
    public sealed class InProcessMessageTransport : ILogMessageStream, ILogMessageCaster
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="InProcessMessageTransport"/> type.
        /// </summary>
        public InProcessMessageTransport()
        {
            BlockingMessageCache = new BlockingCollection<LogMessage>(1000);
            ObservableMessageCache = CreateObservableMessageCache();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="InProcessMessageTransport"/> type.
        /// </summary>
        /// <param name="capacity">
        /// The maximum number of log messages that are cached in-memory. The subcribed <see cref="ILogMessageStore"/>
        /// instances will asynchronously consume and persist those messages. However if the consumption is too slow
        /// and the maximum capacity is exceeded further calls to <see cref="ILogMessageCaster.Cast(LogMessage)"/>
        /// will block and be processed synchronously.
        /// </param>
        public InProcessMessageTransport(uint capacity)
        {
            BlockingMessageCache = new BlockingCollection<LogMessage>((int)capacity);
            ObservableMessageCache = CreateObservableMessageCache();
        }

        #endregion

        #region Data

        /// <summary>
        /// Gets a bounded buffer that caches a maximum number of <see cref="LogMessage"/> in-memory
        /// before the subscribed <see cref="ILogMessageStore"/> instances will persist those messages
        /// asynchronously.
        /// </summary>
        private BlockingCollection<LogMessage> BlockingMessageCache { get; }

        /// <summary>
        /// Gets an <see cref="IObservable{T}"/> version of the <see cref="BlockingMessageCache"/>.
        /// </summary>
        private Lazy<IObservable<LogMessage>> ObservableMessageCache { get; }

        #endregion

        #region Logic

        /// <inheritdoc />
        public void Cast(LogMessage value)
        {
            BlockingMessageCache.Add(value);
        }

        /// <inheritdoc />
        public IDisposable Subscribe(IObserver<LogMessage> observer)
        {
            return ObservableMessageCache.Value.Subscribe(observer);
        }

        /// <summary>
        /// Creates an observable representation of the <see cref="BlockingMessageCache"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="IObservable{T}"/> representation of the <see cref="BlockingMessageCache"/>.
        /// </returns>
        private Lazy<IObservable<LogMessage>> CreateObservableMessageCache()
        {
            return new Lazy<IObservable<LogMessage>>(() =>
                {
                    return BlockingMessageCache
                        .GetConsumingEnumerable()
                        .ToObservable(TaskPoolScheduler.Default)
                        .Publish()
                        .RefCount();
                }, true);
        }

        #endregion
    }
}