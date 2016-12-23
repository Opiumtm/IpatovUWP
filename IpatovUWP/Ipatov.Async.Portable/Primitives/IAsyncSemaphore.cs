namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async semaphore.
    /// </summary>
    public interface IAsyncSemaphore : IAsyncWaitRegion
    {
        /// <summary>
        /// Release semaphore from any thread (does not need release handle).
        /// </summary>
        /// <param name="releaseCount">Count to release.</param>
        /// <returns>Previous semaphore count.</returns>
        int ReleaseSemaphore(int releaseCount);
    }
}