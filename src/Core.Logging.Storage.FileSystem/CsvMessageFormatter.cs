namespace CustomCode.Core.Logging.Storage
{
    using System;

    /// <summary>
    /// Implementation of the default <see cref="ILogMessageFormatter"/> for the
    /// <see cref="FileSystemMessageStore"/>.
    /// </summary>
    public sealed class CsvMessageFormatter : ILogMessageFormatter, IHasFileHeader
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="CsvMessageFormatter"/> type.
        /// </summary>
        /// <param name="separator"> The separator that should be used for the .csv file. </param>
        public CsvMessageFormatter(string? separator = null)
        {
            Separator = separator ?? "|";
            Header = CreateHeader(Separator);
        }

        #endregion

        #region Data

        /// <inheritdoc />
        public string Header { get; }

        /// <summary>
        /// Gets the separator that is used by the .csv file.
        /// </summary>
        public string Separator { get; }

        #endregion

        #region Logic

        /// <inheritdoc />
        public string Format(LogMessage message)
        {
            string result;
            if (message.Exception == null)
            {
                result = string.Join(
                    Separator,
                    message.Timestamp.ToUniversalTime().ToString("dd-MM-yyyy HH:mm:ss"),
                    message.Severity,
                    $"{message.Message}",
                    message.CallingType,
                    message.CallingMethod,
                    message.LineNumber,
                    message.SourceFile,
                    null);
            }
            else
            {
                result = string.Join(
                    Separator,
                    message.Timestamp.ToUniversalTime().ToString("dd-MM-yyyy HH:mm:ss"),
                    message.Severity,
                    $"{message.Message}",
                    message.CallingType,
                    message.CallingMethod,
                    message.LineNumber,
                    message.SourceFile,
                    message.Exception);
            }

            return result;
        }

        /// <summary>
        /// Creates the formatted header for a new .csv file.
        /// </summary>
        /// <param name="separator"> The separator that should be used. </param>
        /// <returns> The .csv file's formatted header. </returns>
        private string CreateHeader(string separator)
        {
            var excelHeader = $"\"sep={separator}\"";
            var columnHeader = string.Join(
                    separator,
                    nameof(LogMessage.Timestamp),
                    nameof(LogMessage.Severity),
                    nameof(LogMessage.Message),
                    nameof(LogMessage.CallingType),
                    nameof(LogMessage.CallingMethod),
                    nameof(LogMessage.LineNumber),
                    nameof(LogMessage.SourceFile),
                    nameof(LogMessage.Exception));
            return $"{excelHeader}{Environment.NewLine}{columnHeader}";
        }

        #endregion
    }
}