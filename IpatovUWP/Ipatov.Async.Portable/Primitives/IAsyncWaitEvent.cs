namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Wait event.
    /// </summary>
    public interface IAsyncWaitEvent : IAsyncWaitContext
    {
        /// <summary>
        /// Set event.
        /// </summary>
        void Set();

        /// <summary>
        /// Reset event.
        /// </summary>
        void Reset();
    }
}