namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Stateless signal that trigger waiters only on set, does not remember previous set state.
    /// </summary>
    public sealed class AsyncStatelessSignal : AsyncSignalBase
    {
    }
}