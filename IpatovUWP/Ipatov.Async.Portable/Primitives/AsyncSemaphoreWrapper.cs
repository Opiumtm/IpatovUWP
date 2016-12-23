using System.Threading;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async semaphore wrapper.
    /// </summary>
    public sealed class AsyncSemaphoreWrapper : WaitHandleWrapper<Semaphore>, IAsyncSemaphore
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="handle">Semaphore.</param>
        public AsyncSemaphoreWrapper(Semaphore handle) : base(handle)
        {
        }

        /// <summary>
        /// Release wait context.
        /// </summary>
        /// <param name="handle">Task handle.</param>
        public void Release(int handle)
        {
            Handle.Release();
        }

        /// <summary>
        /// Release semaphore from any thread (does not need release handle).
        /// </summary>
        /// <param name="releaseCount">Count to release.</param>
        /// <returns>Previous semaphore count.</returns>
        public int ReleaseSemaphore(int releaseCount)
        {
            return Handle.Release(releaseCount);
        }
    }
}