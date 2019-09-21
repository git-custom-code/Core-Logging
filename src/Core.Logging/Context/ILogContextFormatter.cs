namespace CustomCode.Core.Logging.Context
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface that can be used to apply a custom format to the <see cref="LogContext"/>'s
    /// content before it is persisted by a <see cref="ILogMessageStore"/> implementation.
    /// </summary>
    public interface ILogContextFormatter
    {
        /// <summary>
        /// Apply a custom format to the <see cref="LogContext"/>'s content before it is persisted.
        /// </summary>
        /// <param name="context"> The context to be formatted. </param>
        /// <returns> The formatted <paramref name="context"/>. </returns>
        string Format(IEnumerable<object> context);
    }
}