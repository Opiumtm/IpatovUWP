using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async semaphore.
    /// </summary>
    public sealed class AsyncSemaphore : IAsyncSemaphore
    {
        private int _releaseCount;

        private readonly int _maxReleaseCount;

        private readonly object _lock = new object();

        private readonly Queue<WaiterInfo> _waiters = new Queue<WaiterInfo>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="releaseCount">Initial release count.</param>
        /// <param name="maxReleaseCount">Maximum release count.</param>
        public AsyncSemaphore(int releaseCount, int maxReleaseCount)
        {
            if (maxReleaseCount < 1)
            {
                throw new ArgumentException($"Invalid {nameof(maxReleaseCount)} value = {maxReleaseCount}", nameof(maxReleaseCount));
            }
            _releaseCount = Math.Max(Math.Min(releaseCount, maxReleaseCount), 0);
            _maxReleaseCount = maxReleaseCount;
        }

        /// <summary>
        /// Wait on async handle.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task to wait with task handle.</returns>
        public async Task<int> Wait(CancellationToken cancellationToken)
        {
            WaiterInfo info = null;
            bool isFree = false;
            lock (_lock)
            {
                if (_releaseCount > 0)
                {
                    _releaseCount--;
                    isFree = true;
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
            if (isFree)
            {
                return -1;
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
        public async Task Release(int handle)
        {
            await ReleaseSemaphore(1);
        }

        /// <summary>
        /// Release semaphore from any thread (does not need release handle).
        /// </summary>
        /// <param name="releaseCount">Count to release.</param>
        /// <returns>Previous semaphore count.</returns>
        public async Task<int> ReleaseSemaphore(int releaseCount)
        {
            if (releaseCount < 1)
            {
                throw new ArgumentException("Invalid release count", nameof(releaseCount));
            }
            int old;
            var toRelease = new List<WaiterInfo>();
            lock (_lock)
            {
                old = _releaseCount;
                _releaseCount += releaseCount;
                if (_releaseCount > _maxReleaseCount)
                {
                    _releaseCount = _maxReleaseCount;
                }
                while (_waiters.Count > 0 && _releaseCount > 0)
                {
                    var next = _waiters.Dequeue();
                    _releaseCount--;
                    if (!next.isCancelled)
                    {
                        next.isScheduled = true;
                        toRelease.Add(next);
                    }
                }
                _releaseCount += releaseCount;
            }
            await Task.Run(() =>
            {
                foreach (var wi in toRelease)
                {
                    try
                    {
                        wi.tcs.TrySetResult(-1);
                    }
                    catch
                    {
                        // ignore
                    }
                }
            });
            return old;
        }

        private class WaiterInfo
        {
            public TaskCompletionSource<int> tcs;
            public bool isCancelled;
            public bool isScheduled;
        }
    }
}