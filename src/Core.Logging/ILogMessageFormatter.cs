namespace CustomCode.Core.Logging
{
    /// <summary>
    /// Interface that can be used to apply a custom format to a <see cref="LogMessage"/>
    /// before it is persisted by a <see cref="ILogMessageStore"/> implementation.
    /// </summary>
    public interface ILogMessageFormatter
    {
        /// <summary>
        /// Apply a custom format to a <see cref="LogMessage"/> before it is persisted.
        /// </summary>
        /// <param name="message"> The message to be formatted. </param>
        /// <returns> The formatted message. </returns>
        string Format(LogMessage message);
    }
}