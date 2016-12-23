using System.Threading;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Auto reset event wrapper.
    /// </summary>
    public sealed class AsyncAutoResetEventWrapper : WaitHandleWrapper<AutoResetEvent>, IAsyncSignal
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="handle">Auto reset event.</param>
        public AsyncAutoResetEventWrapper(AutoResetEvent handle) : base(handle)
        {
        }

        /// <summary>
        /// Set gate to triggered state.
        /// </summary>
        /// <returns>true if successful.</returns>
        public bool Set()
        {
            return Handle.Set();
        }
    }
}