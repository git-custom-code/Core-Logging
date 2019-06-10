namespace CustomCode.Core.Logging
{
    using LightInject;
    using Transport;

    /// <summary>
    /// Extension methods for the <see cref="IServiceRegistry"/> type that can be used to
    /// enable in-process log message transport.
    /// </summary>
    public static class IServiceRegistryExtensions
    {
        #region Logic

        /// <summary>
        /// Register the <see cref="InProcessMessageTransport"/> using an optional maximum capacity
        /// thereby enabling in-process log message transport.
        /// </summary>
        /// <param name="iocContainer"> The extended <see cref="IServiceRegistry"/>. </param>
        /// <param name="capacity">
        /// The maximum number of log messages that are cached in-memory. The subcribed <see cref="ILogMessageStore"/>
        /// instances will asynchronously consume and persist those messages. However if the consumption is too slow
        /// and the maximum capacity is exceeded further calls to <see cref="ILogMessageCaster.Cast(LogMessage)"/>
        /// will block and be processed synchronously.
        /// </param>
        public static void RegisterInProcessLogging(this IServiceRegistry iocContainer, uint capacity = 1000)
        {
            var transport = new InProcessMessageTransport(capacity);
            iocContainer.Register<ILogMessageCaster>(
                factory => transport,
                nameof(InProcessMessageTransport),
                new PerContainerLifetime());
            iocContainer.Register<ILogMessageStream>(
                factory => transport,
                nameof(InProcessMessageTransport),
                new PerContainerLifetime());
        }

        #endregion
    }
}