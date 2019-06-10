namespace CustomCode.Core.Logging
{
    /// <summary>
    /// Interface that is called by <see cref="ILog"/> implementations and will cast
    /// <see cref="LogMessage"/> instances to a connected <see cref="ILogMessageStream"/>.
    /// </summary>
    public interface ILogMessageCaster
    {
        /// <summary>
        /// Cast a new <see cref="LogMessage"/> to the connected <see cref="ILogMessageStream"/>.
        /// </summary>
        /// <param name="message"> The message to be logged. </param>
        void Cast(LogMessage message);
    }
}