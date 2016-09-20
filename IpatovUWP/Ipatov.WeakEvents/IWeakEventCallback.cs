namespace Ipatov.WeakEvents
{
    /// <summary>
    /// Weak event callback.
    /// </summary>
    public interface IWeakEventCallback
    {
        /// <summary>
        /// Receive weak event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event argument</param>
        /// <param name="channel">Event channel</param>
        void ReceiveWeakEvent(object sender, IWeakEventChannel channel, object e);
    }
}