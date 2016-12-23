using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Wait handle wrapper.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WaitHandleWrapper<T> : IAsyncWaitContext, IDisposable where T : WaitHandle
    {
        /// <summary>
        /// Wrapped wait handle.
        /// </summary>
        public T Handle { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="handle">Handle.</param>
        public WaitHandleWrapper(T handle)
        {
            if (handle == null) throw new ArgumentNullException(nameof(handle));
            Handle = handle;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Handle.Dispose();
        }

        /// <summary>
        /// Wait on async handle.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task to wait with task handle.</returns>
        public Task<int> Wait(CancellationToken cancellationToken)
        {
            return WaitCore(cancellationToken);
        }

        /// <summary>
        /// Wait on async handle.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task to wait with task handle.</returns>
        protected virtual Task<int> WaitCore(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var toWait = new WaitHandle[] { Handle, cancellationToken.WaitHandle };
                var idx = WaitHandle.WaitAny(toWait);
                switch (idx)
                {
                    case 1:
                        throw new OperationCanceledException(cancellationToken);
                    default:
                        return -1;
                }
            }, cancellationToken);
        }
    }
}