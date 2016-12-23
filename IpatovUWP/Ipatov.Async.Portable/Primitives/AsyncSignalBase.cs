using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async signal base class.
    /// </summary>
    public abstract class AsyncSignalBase : IAsyncSignal
    {
        private List<TaskCompletionSource<int>> _waiters = new List<TaskCompletionSource<int>>();

        private SpinLock _lock = new SpinLock();

        /// <summary>
        /// Wait on async handle.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task to wait with task handle.</returns>
        public virtual async Task<int> Wait(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<int>();
            var task = tcs.Task;
            var taken = false;
            _lock.Enter(ref taken);
            if (taken)
            {
                try
                {
                    _waiters.Add(tcs);
                }
                finally
                {
                    _lock.Exit();
                }
            }
            else
            {
                throw new InvalidOperationException("Unable to take spin lock");
            }

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
            var taken = false;
            _lock.Enter(ref taken);
            List<TaskCompletionSource<int>> watiers;
            List<TaskCompletionSource<int>> newWaiters = new List<TaskCompletionSource<int>>();
            if (taken)
            {
                try
                {
                    watiers = _waiters;
                    _waiters = newWaiters;
                }
                finally
                {
                    _lock.Exit();
                }
            }
            else
            {
                return false;
            }
            foreach (var waiter in watiers)
            {
                waiter.TrySetResult(-1);
            }
            return true;
        }
    }
}