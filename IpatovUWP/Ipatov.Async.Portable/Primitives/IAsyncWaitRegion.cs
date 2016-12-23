namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async wait region (context with release).
    /// </summary>
    public interface IAsyncWaitRegion : IAsyncWaitContext
    {
        /// <summary>
        /// Release wait context.
        /// </summary>
        /// <param name="handle">Task handle.</param>
        void Release(int handle);
    }
}