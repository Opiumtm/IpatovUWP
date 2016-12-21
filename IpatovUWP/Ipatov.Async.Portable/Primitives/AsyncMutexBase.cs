using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async exclusive mutex.
    /// </summary>
    public abstract class AsyncMutexBase : IAsyncAccessContext
    {
        private readonly object _lock = new object();

        private readonly Dictionary<AsyncSchedulePriority, List<CompletionInfo>> _waiters = new Dictionary<AsyncSchedulePriority, List<CompletionInfo>>();

        private readonly HashSet<Guid> _activeLocks = new HashSet<Guid>();

        /// <summary>
        /// Return if blocking is needed.
        /// </summary>
        /// <param name="count">Active locks count.</param>
        /// <returns>Result.</returns>
        protected abstract bool IsBlocking(int count);

        /// <summary>
        /// Request access async.
        /// </summary>
        /// <param name="priority">Priority.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Disposable token to release access.</returns>
        public Task<IDisposable> RequestAccess(AsyncSchedulePriority priority, CancellationToken cancellationToken)
        {
            using (var toDo = new ActionList(null))
            {
                lock (_lock)
                {
                    if (!IsBlocking(_activeLocks.Count))
                    {
                        var id = Guid.NewGuid();
                        _activeLocks.Add(id);
                        return Task.FromResult(CreateReleaseDisposableNoLock(id));
                    }
                    var tcs = new TaskCompletionSource<IDisposable>();
                    var info = new CompletionInfo(tcs, Guid.NewGuid(), priority);
                    toDo.Register(() =>
                    {
                        cancellationToken.Register(CancellationHandler(info));
                    });
                    Enqueue(info);
                    return tcs.Task;
                }
            }
        }

        private Action CancellationHandler(CompletionInfo info)
        {
            return () =>
            {
                if (info.Completion.TrySetCanceled())
                {
                    lock (_lock)
                    {
                        Remove(info);
                    }
                }
            };
        }

        private void Remove(CompletionInfo info)
        {
            if (_waiters.ContainsKey(info.Priority))
            {
                _waiters[info.Priority].Remove(info);
            }
        }

        /// <summary>
        /// Dequeue element.
        /// </summary>
        /// <returns>Element.</returns>
        private CompletionInfo Dequeue()
        {
            foreach (var key in _waiters.Keys.OrderByDescending(p => (int) p))
            {
                var r = Dequeue(_waiters[key]);
                if (r != null)
                {
                    return r;
                }
            }
            return null;
        }

        private CompletionInfo Dequeue(List<CompletionInfo> waiters)
        {
            if (waiters.Count > 0)
            {
                var r = waiters[0];
                waiters.RemoveAt(0);
                return r;
            }
            return null;
        }

        private void Enqueue(CompletionInfo info)
        {
            if (!_waiters.ContainsKey(info.Priority))
            {
                _waiters[info.Priority] = new List<CompletionInfo>();
            }
            _waiters[info.Priority].Add(info);
        }

        private void Release(Guid lockId)
        {
            using (var toDo = new ActionList(null))
            {
                lock (_lock)
                {
                    _activeLocks.Remove(lockId);
                    if (!IsBlocking(_activeLocks.Count))
                    {
                        var top = Dequeue();
                        if (top != null)
                        {
                            toDo.Register(ScheduleNextWaiter(top));
                        }
                    }
                }
            }
        }

        private IDisposable CreateReleaseDisposable(Guid id)
        {
            lock (_lock)
            {
                return CreateReleaseDisposableNoLock(id);
            }
        }

        private IDisposable CreateReleaseDisposableNoLock(Guid id)
        {
            _activeLocks.Add(id);
            return new ActionDisposable(() =>
            {
                Release(id);
            });
        }

        private Action ScheduleNextWaiter(CompletionInfo tcs)
        {
            return () =>
            {
                if (!tcs.Completion.TrySetResult(CreateReleaseDisposable(tcs.Id)))
                {
                    Release(tcs.Id);
                }
            };
        }

        private class CompletionInfo
        {
            public CompletionInfo(TaskCompletionSource<IDisposable> completion, Guid id, AsyncSchedulePriority priority)
            {
                Completion = completion;
                Id = id;
                Priority = priority;
            }

            public readonly TaskCompletionSource<IDisposable> Completion;

            public readonly Guid Id;

            public readonly AsyncSchedulePriority Priority;
        }
    }
}