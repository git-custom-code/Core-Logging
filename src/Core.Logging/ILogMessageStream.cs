namespace CustomCode.Core.Logging
{
    using System;

    /// <summary>
    /// Interface for an observable stream of <see cref="LogMessage"/> instances.
    /// </summary>
    /// <remarks>
    /// <see cref="ILogMessageStore"/> implmentations will subscribe to this stream
    /// and log any send <see cref="LogMessage"/>.
    /// </remarks>
    public interface ILogMessageStream : IObservable<LogMessage>
    { }
}