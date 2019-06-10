namespace CustomCode.Core.Logging
{
    using LightInject;
    using Storage;
    using System;
    using System.IO;

    /// <summary>
    /// Extension methods for the <see cref="IServiceRegistry"/> type that can be used to
    /// enable logging to a log file on the hard disk.
    /// </summary>
    public static class IServiceRegistryExtensions
    {
        #region Logic

        /// <summary>
        /// Register the <see cref="FileSystemMessageStore"/> using it's default csv formatter thereby
        /// enabling logging to a .csv log file on the hard disk.
        /// </summary>
        /// <param name="iocContainer"> The extended <see cref="IServiceRegistry"/>. </param>
        /// <param name="csvFilePath"> The path to the .csv log file. </param>
        /// <param name="separator"> The separator that should be used by the .csv log file. </param>
        public static void RegisterCsvFileSystemLogging(this IServiceRegistry iocContainer,
            string csvFilePath, string separator = "|")
        {
            FileSystemMessageStore CreateFileSystemMessageStore(IServiceFactory factory)
            {
                var messageStream = factory.GetInstance<ILogMessageStream>();
                var formatter = factory.TryGetInstance<ILogMessageFormatter?>() ?? new CsvMessageFormatter(separator);
                if (formatter is IHasFileHeader formatterWithHeader)
                {
                    return new FileSystemMessageStore(
                        messageStream,
                        csvFilePath,
                        message => formatter.Format(message),
                        () => formatterWithHeader.Header);
                }

                return new FileSystemMessageStore(
                    messageStream,
                    csvFilePath,
                    message => formatter.Format(message));
            }

            if (string.IsNullOrWhiteSpace(csvFilePath))
            {
                throw new ArgumentException(nameof(csvFilePath));
            }

            if (string.IsNullOrEmpty(Path.GetFileName(csvFilePath)))
            {
                csvFilePath = Path.Combine(csvFilePath, "Log.csv");
            }

            if (Path.GetExtension(csvFilePath) != ".csv")
            {
                csvFilePath = Path.ChangeExtension(csvFilePath, ".csv");
            }

            iocContainer.Register<ILogMessageStore>(
                CreateFileSystemMessageStore,
                csvFilePath,
                new PerContainerLifetime());
        }

        /// <summary>
        /// Register the <see cref="FileSystemMessageStore"/> using a custom formatter
        /// thereby enabling logging to a log file on the hard disk.
        /// </summary>
        /// <param name="iocContainer"> The extended <see cref="IServiceRegistry"/>. </param>
        /// <param name="filePath"> The path to the log file. </param>
        /// <param name="formatter"> The custom message formatter to be used. </param>
        public static void RegisterFileSystemLogging(this IServiceRegistry iocContainer,
            string filePath, ILogMessageFormatter formatter)
        {
            FileSystemMessageStore CreateFileSystemMessageStore(IServiceFactory factory)
            {
                var messageStream = factory.GetInstance<ILogMessageStream>();
                if (formatter is IHasFileHeader formatterWithHeader)
                {
                    return new FileSystemMessageStore(
                        messageStream,
                        filePath,
                        message => formatter.Format(message),
                        () => formatterWithHeader.Header);
                }

                return new FileSystemMessageStore(
                        messageStream,
                        filePath,
                        message => formatter.Format(message));
            }

            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException(nameof(filePath));
            }

            iocContainer.Register<ILogMessageStore>(
                CreateFileSystemMessageStore,
                filePath,
                new PerContainerLifetime());
        }

        /// <summary>
        /// Register the <see cref="FileSystemMessageStore"/> using a custom delegate for formatting messages
        /// thereby enabling logging to a log file on the hard disk.
        /// </summary>
        /// <param name="iocContainer"> The extended <see cref="IServiceRegistry"/>. </param>
        /// <param name="filePath"> The path to the log file. </param>
        /// <param name="formatMessageAction">
        /// A custom delegate that will be used to format messages.
        /// </param>
        /// <param name="headerAction"> An optional delegate that is used as a custom file header. </param>
        public static void RegisterFileSystemLogging(this IServiceRegistry iocContainer,
            string filePath, Func<LogMessage, string> formatMessageAction, Func<string>? headerAction = null)
        {
            FileSystemMessageStore CreateFileSystemMessageStore(IServiceFactory factory)
            {
                var messageStream = factory.GetInstance<ILogMessageStream>();
                return new FileSystemMessageStore(
                    messageStream,
                    filePath,
                    formatMessageAction,
                    headerAction);
            }

            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException(nameof(filePath));
            }

            iocContainer.Register<ILogMessageStore>(
                CreateFileSystemMessageStore,
                filePath,
                new PerContainerLifetime());
        }

        #endregion
    }
}