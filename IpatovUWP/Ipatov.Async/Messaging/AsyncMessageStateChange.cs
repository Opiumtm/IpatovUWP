namespace Ipatov.Async.Messaging
{
    /// <summary>
    /// State change.
    /// </summary>
    public enum AsyncMessageStateChange
    {
        /// <summary>
        /// Replied.
        /// </summary>
        Replied,
        /// <summary>
        /// Faulted.
        /// </summary>
        Faulted,
        /// <summary>
        /// Canceled.
        /// </summary>
        Canceled
    }
}