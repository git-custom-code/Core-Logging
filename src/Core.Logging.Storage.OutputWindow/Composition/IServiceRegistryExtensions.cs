namespace CustomCode.Core.Logging
{
    using LightInject;
    using Storage;
    using System;

    /// <summary>
    /// Extension methods for the <see cref="IServiceRegistry"/> type that can be used to
    /// enable logging to the output window of the attached debugger (visual studio).
    /// </summary>
    public static class IServiceRegistryExtensions
    {
        #region Logic

        /// <summary>
        /// Register the <see cref="OutputWindowMessageStore"/> using it's default formatter thereby
        /// enabling logging to the output window of the attached debugger (visual studio).
        /// </summary>
        /// <param name="iocContainer"> The extended <see cref="IServiceRegistry"/>. </param>
        public static void RegisterOutputWindowLogging(this IServiceRegistry iocContainer)
        {
            OutputWindowMessageStore CreateDebuggerMessageStore(IServiceFactory factory)
            {
                var messageStream = factory.GetInstance<ILogMessageStream>();
                var formatter = factory.TryGetInstance<ILogMessageFormatter?>() ?? new OutputWindowMessageFormatter();
                return new OutputWindowMessageStore(messageStream, message => formatter.Format(message));
            }

            iocContainer.Register<ILogMessageStore>(
                CreateDebuggerMessageStore,
                nameof(OutputWindowMessageStore),
                new PerContainerLifetime());
        }

        /// <summary>
        /// Register the <see cref="OutputWindowMessageStore"/> using a custom formatter thereby
        /// enabling logging to the output window of the attached debugger (visual studio).
        /// </summary>
        /// <param name="iocContainer"> The extended <see cref="IServiceRegistry"/>. </param>
        /// <param name="formatter"> The custom message formatter to be used. </param>
        public static void RegisterOutputWindowLogging(this IServiceRegistry iocContainer, ILogMessageFormatter formatter)
        {
            OutputWindowMessageStore CreateDebuggerMessageStore(IServiceFactory factory)
            {
                var messageStream = factory.GetInstance<ILogMessageStream>();
                return new OutputWindowMessageStore(messageStream, message => formatter.Format(message));
            }

            iocContainer.Register<ILogMessageStore>(
                CreateDebuggerMessageStore,
                nameof(OutputWindowMessageStore),
                new PerContainerLifetime());
        }

        /// <summary>
        /// Register the <see cref="OutputWindowMessageStore"/> using a custom delegate for formatting messages
        /// thereby enabling logging to the output window of the attached debugger (visual studio).
        /// </summary>
        /// <param name="iocContainer"> The extended <see cref="IServiceRegistry"/>. </param>
        /// <param name="formatMessage"> A custom delegate that will be used to format messages. </param>
        public static void RegisterOutputWindowLogging(this IServiceRegistry iocContainer, Func<LogMessage, string> formatMessage)
        {
            OutputWindowMessageStore CreateDebuggerMessageStore(IServiceFactory factory)
            {
                var messageStream = factory.GetInstance<ILogMessageStream>();
                return new OutputWindowMessageStore(messageStream, formatMessage);
            }

            iocContainer.Register<ILogMessageStore>(
                CreateDebuggerMessageStore,
                nameof(OutputWindowMessageStore),
                new PerContainerLifetime());
        }

        #endregion
    }
}