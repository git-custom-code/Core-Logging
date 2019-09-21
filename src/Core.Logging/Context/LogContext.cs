namespace CustomCode.Core.Logging
{
    using Context;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Gives developers the possibility to add contextual information to subsequent <see cref="Log"/>
    /// method calls (see example below for usage).
    /// </summary>
    /// <example>
    /// The intended usage is to create a <see cref="LogContext"/> instance only inside of using
    /// blocks (like with any instances that implement <see cref="IDisposable"/>) and use an anonymous
    /// class with as many properties as you like. All of <see cref="ILog"/>'s method calls that are
    /// made within the using's scope will add the property values from the <see cref="LogContext"/>
    /// to each persisted <see cref="LogMessage"/>.
    /// 
    /// <![CDATA[
    /// var contextInt = 42;
    /// var contextString = "Context";
    /// using (var context = new LogContext(new { Key1 = contextInt, Key2 = contextString }))
    /// {
    ///     Log.Information("Test"); // this will add "Key1 = 42" and "Key2 = "Context"" to the log message
    /// }
    /// ]]>
    /// </example>
    public sealed class LogContext : IDisposable
    {
        #region Dependencies

        /// <summary>
        /// Creates a new instance of the <see cref="LogContext"/> type.
        /// </summary>
        /// <param name="context">
        /// The context that will be added to each <see cref="LogMessage"/> that is persisted while
        /// this instance is "in scope".
        /// </param>
        public LogContext(object context)
        {
            AmbientContextStack.Push(context);
        }

        #endregion

        #region Data

        /// <summary>
        /// Gets a snapshot of the current <see cref="LogContext"/>.
        /// </summary>
        public IEnumerable<object> Current
        {
            get { return AmbientContextStack.CreateSnapshot(); }
        }

        #endregion

        #region Logic

        /// <summary>
        /// Dispose this instance.
        /// </summary>
        /// <remarks>
        /// Note that this will remove this instance's context from the <see cref="AmbientContextStack"/>.
        /// </remarks>
        public void Dispose()
        {
            if (Marshal.GetLastWin32Error() == 0)
            {
                AmbientContextStack.Pop();
            }
        }

        #endregion
    }
}