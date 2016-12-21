namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async mutex.
    /// </summary>
    public sealed class AsyncMutex : AsyncMutexBase
    {
        /// <summary>
        /// Return if blocking is needed.
        /// </summary>
        /// <param name="count">Active locks count.</param>
        /// <returns>Result.</returns>
        protected override bool IsBlocking(int count) => count < 1;
    }
}