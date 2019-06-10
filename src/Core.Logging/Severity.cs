namespace CustomCode.Core.Logging
{
    /// <summary>
    /// Enumeration for the available <see cref="LogMessage"/> severities.
    /// </summary>
    public enum Severity : byte
    {
        /// <summary> The message contains debug relevant and maybe security critical content. </summary>
        Debug = 0,
        /// <summary> The message contains informational content. </summary>
        Information = 1,
        /// <summary>
        /// The message contains content that may indicate a potential application problem
        /// or business exception.
        /// </summary>
        Warning = 2,
        /// <summary> The message contains error content. </summary>
        Error = 3
    }
}