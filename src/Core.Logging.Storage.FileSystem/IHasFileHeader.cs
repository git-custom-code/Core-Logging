namespace CustomCode.Core.Logging.Storage
{
    /// <summary>
    /// Interface that can be used in addition to a <see cref="ILogMessageFormatter"/> in order
    /// to define if the persisted log file has a file header.
    /// </summary>
    public interface IHasFileHeader
    {
        /// <summary>
        /// Gets the log file's formatted header.
        /// </summary>
        string Header { get; }
    }
}