using System;

namespace Ipatov.WeakEvents
{
    /// <summary>
    /// Weak event channel
    /// </summary>
    public interface IWeakEventChannel
    {
        /// <summary>
        /// Channel identifier.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Register callback.
        /// </summary>
        /// <param name="callback">Weak event callback.</param>
        /// <returns>Callback registration token.</returns>
        Guid AddCallback(IWeakEventCallback callback);

        /// <summary>
        /// Remove callback.
        /// </summary>
        /// <param name="token">Weak event callback registration token.</param>
        void RemoveCallback(Guid token);

        /// <summary>
        /// Trigger weak event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event argument.</param>
        void RaiseEvent(object sender, object e);
    }
}
