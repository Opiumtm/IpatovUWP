using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async signal base class.
    /// </summary>
    public abstract class AsyncSignalBase : IAsyncSignal
    {
        private readonly ConcurrentBag<TaskCompletionSource<int>> _waiters = new ConcurrentBag<TaskCompletionSource<int>>();

        /// <summary>
        /// Wait on async handle.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task to wait with task handle.</returns>
        public virtual async Task<int> Wait(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<int>();
            _waiters.Add(tcs);
            var task = tcs.Task;
            var ctr = cancellationToken.Register(() =>
            {
                tcs.TrySetCanceled();
            });
            using (ctr)
            {
                return await task;
            }
        }

        /// <summary>
        /// Trigger signal.
        /// </summary>
        /// <returns>true if successful.</returns>
        public virtual bool Set()
        {
            TaskCompletionSource<int> waiter;
            while (_waiters.TryTake(out waiter))
            {
                waiter.TrySetResult(-1);
            }
            return true;
        }
    }
}