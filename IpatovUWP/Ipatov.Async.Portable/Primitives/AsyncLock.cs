using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async lock (critical section).
    /// </summary>
    public sealed class AsyncLock : IAsyncWaitRegion
    {
        private readonly object _lock = new object();

        private int _autoincId = 0;

        private int? _heldLock = null;

        private readonly Queue<WaiterInfo> _waiters = new Queue<WaiterInfo>();


        /// <summary>
        /// Wait on async handle.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task to wait with task handle.</returns>
        public async Task<int> Wait(CancellationToken cancellationToken)
        {
            int? id = null;
            WaiterInfo info = null;
            lock (_lock)
            {
                if (_heldLock == null)
                {
                    id = Interlocked.Increment(ref _autoincId);
                    _heldLock = id.Value;
                }
                else
                {
                    info = new WaiterInfo()
                    {
                        tcs = new TaskCompletionSource<int>(),
                        isCancelled = false
                    };
                    _waiters.Enqueue(info);
                }
            }
            if (id != null)
            {
                return id.Value;
            }
            using (cancellationToken.Register(() =>
            {
                lock (_lock)
                {
                    if (info.isScheduled)
                    {
                        return;
                    }
                    info.isCancelled = true;
                }
                info.tcs.TrySetCanceled();
            }))
            {
                return await info.tcs.Task;
            }
        }

        /// <summary>
        /// Release wait context.
        /// </summary>
        /// <param name="handle">Task handle.</param>
        public void Release(int handle)
        {
            WaiterInfo next = null;
            int? id = null;
            lock (_lock)
            {
                if (_heldLock != handle || _heldLock == null)
                {
                    throw new InvalidOperationException("Invalid release handle. Lock is held with another waiter.");
                }
                while (_waiters.Count > 0 && next == null)
                {                    
                    next = _waiters.Dequeue();
                    if (next.isCancelled)
                    {
                        next = null;
                    }
                }
                if (next != null)
                {
                    id = Interlocked.Increment(ref _autoincId);
                    _heldLock = id.Value;
                    next.isScheduled = true;
                }
                else
                {
                    _heldLock = null;
                }
            }
            next?.tcs.TrySetResult(id.Value);
        }

        private class WaiterInfo
        {
            public TaskCompletionSource<int> tcs;
            public bool isCancelled;
            public bool isScheduled;
        }
    }
}