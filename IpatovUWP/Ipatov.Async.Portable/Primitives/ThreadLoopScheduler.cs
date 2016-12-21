using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Thread loop scheduler.
    /// </summary>
    public sealed class ThreadLoopScheduler : IAsyncScheduler
    {
        private readonly object _lock = new object();

        private readonly AsyncScheduleQueue<ScheduleInfo> _queue = new AsyncScheduleQueue<ScheduleInfo>();

        /// <summary>
        /// Schedule function.
        /// </summary>
        /// <param name="priority">Priority.</param>
        /// <param name="scheduleFunc">Function to schedule.</param>
        /// <returns>Function completion task.</returns>
        public Task<T> Schedule<T>(AsyncSchedulePriority priority, Func<T> scheduleFunc)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loop until cancellation token indicate loop cancel.
        /// </summary>
        /// <param name="token"></param>
        public void Loop(CancellationToken token)
        {
            for (;;)
            {
                token.ThrowIfCancellationRequested();
            }
        }

        private class ScheduleInfo
        {
            public readonly Func<object, bool> SetResult;
            public readonly Func<bool> SetCancelled;
            public readonly Func<Exception, bool> SetError;
            public readonly AsyncSchedulePriority Priority;

            public ScheduleInfo(Func<object, bool> setResult, Func<bool> setCancelled, Func<Exception, bool> setError, AsyncSchedulePriority priority)
            {
                SetResult = setResult;
                SetCancelled = setCancelled;
                SetError = setError;
                Priority = priority;
            }
        }
    }
}