using System;
using System.Threading.Tasks;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async task scheduler.
    /// </summary>
    public interface IAsyncScheduler
    {
        /// <summary>
        /// Schedule function.
        /// </summary>
        /// <param name="priority">Priority.</param>
        /// <param name="scheduleFunc">Function to schedule.</param>
        /// <returns>Function completion task.</returns>
        Task<T> Schedule<T>(AsyncSchedulePriority priority, Func<T> scheduleFunc);
    }
}