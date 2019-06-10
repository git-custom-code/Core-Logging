namespace CustomCode.Core.Logging.Storage
{
    /// <summary>
    /// Implementation of the default <see cref="ILogMessageFormatter"/> for the
    /// <see cref="OutputWindowMessageStore"/>.
    /// </summary>
    public sealed class OutputWindowMessageFormatter : ILogMessageFormatter
    {
        #region Logic

        /// <inheritdoc />
        public string Format(LogMessage message)
        {
            if (message.Exception == null)
            {
                return $"{message.CallingType}.{message.CallingMethod} ({message.Severity}): {message.Message}";
            }

            return $"{message.CallingType}.{message.CallingMethod} ({message.Severity}): {message.Exception}";
        }

        #endregion
    }
}