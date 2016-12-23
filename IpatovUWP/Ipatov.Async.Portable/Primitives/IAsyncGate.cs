namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async gate (manual reset event).
    /// </summary>
    public interface IAsyncGate : IAsyncSignal
    {
        /// <summary>
        /// Reset gate.
        /// </summary>
        /// <returns>true if successful.</returns>
        bool Reset();
    }
}