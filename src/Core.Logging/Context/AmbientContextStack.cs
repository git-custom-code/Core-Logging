namespace CustomCode.Core.Logging.Context
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Implementation of a concurrent stack that "flows" across asynchronous operations.
    /// </summary>
    internal static class AmbientContextStack
    {
        #region Data

        /// <summary>
        /// Gets an ambient stack that "flows" across asynchronous operations.
        /// </summary>
        private static AsyncLocal<ConcurrentStack<object>?> AmbientStack { get; }
            = new AsyncLocal<ConcurrentStack<object>?>();

        #endregion

        #region Logic

        /// <summary>
        /// Creates a snapshot of the ambient context stack's items.
        /// </summary>
        /// <returns> A snapshot of the ambient context stack's items. </returns>
        internal static IEnumerable<object> CreateSnapshot()
        {
            var stack = AmbientStack.Value;
            if (stack != null)
            {
                return stack.ToArray();
            }

            return Enumerable.Empty<object>();
        }

        /// <summary>
        /// Pops the top item from the ambient stack.
        /// </summary>
        /// <returns> The stack's top item or null if the stack is empty. </returns>
        internal static object? Pop()
        {
            var stack = AmbientStack.Value;
            if (stack != null)
            {
                if (stack.TryPop(out var value))
                {
                    return value;
                }
            }

            return null;
        }

        /// <summary>
        /// Pushes a new <paramref name="item"/> to the ambient stack.
        /// </summary>
        /// <param name="item"> The data that should be pushed to the stack. </param>
        internal static void Push(object item)
        {
            var stack = AmbientStack.Value;
            if (stack != null)
            {
                stack.Push(item);
            }
            else
            {
                stack = new ConcurrentStack<object>();
                stack.Push(item);
                AmbientStack.Value = stack;
            }
        }

        #endregion
    }
}