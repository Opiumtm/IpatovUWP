using System.Threading;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Manual reset event wrapper.
    /// </summary>
    public sealed class AsyncManualResetEventWrapper : WaitHandleWrapper<ManualResetEvent>, IAsyncGate
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="handle">Manual reset event.</param>
        public AsyncManualResetEventWrapper(ManualResetEvent handle) : base(handle)
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

        /// <summary>
        /// Reset gate.
        /// </summary>
        /// <returns>true if successful.</returns>
        public bool Reset()
        {
            return Handle.Reset();
        }
    }
}