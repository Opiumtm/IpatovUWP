namespace Ipatov.Async.Messaging
{
    /// <summary>
    /// Blocking channel behavior on disposed state.
    /// </summary>
    public enum BlockingChannelDisposedMode
    {
        /// <summary>
        /// Discard messages.
        /// </summary>
        Discard,

        /// <summary>
        /// Immediate exception.
        /// </summary>
        ImmediateException,

        /// <summary>
        /// Return faluted task.
        /// </summary>
        FaultedTask
    }
}