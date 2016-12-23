namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async signal (auto-reset event).
    /// </summary>
    public interface IAsyncSignal : IAsyncWaitContext
    {
        /// <summary>
        /// Trigger signal.
        /// </summary>
        /// <returns>true if successful.</returns>
        bool Set();
    }
}