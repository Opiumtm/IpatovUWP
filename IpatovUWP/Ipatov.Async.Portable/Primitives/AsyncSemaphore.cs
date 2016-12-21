using System;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async semaphore.
    /// </summary>
    public sealed class AsyncSemaphore : AsyncMutexBase
    {
        private readonly int _maxCount;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="maxCount">Maximum concurrent tasks.</param>
        public AsyncSemaphore(int maxCount)
        {
            if (maxCount < 1)
            {
                throw new ArgumentException("Maximum semaphore count must be > 0", nameof(maxCount));
            }
            _maxCount = maxCount;
        }

        /// <summary>
        /// Return if blocking is needed.
        /// </summary>
        /// <param name="count">Active locks count.</param>
        /// <returns>Result.</returns>
        protected override bool IsBlocking(int count) => count < _maxCount;
    }
}