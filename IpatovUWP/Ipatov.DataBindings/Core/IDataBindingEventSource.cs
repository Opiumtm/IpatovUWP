using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding event source.
    /// </summary>
    /// <typeparam name="T">Bound object type.</typeparam>
    public interface IDataBindingEventSource<out T> : IDisposable
    {
        /// <summary>
        /// Add data binding callback.
        /// </summary>
        /// <param name="callback">Callback.</param>
        /// <returns>Callback registration token.</returns>
        Guid AddCallback(IDataBindingEventCallback callback);

        /// <summary>
        /// Remove data binding callback.
        /// </summary>
        /// <param name="callbackToken">Callback registration token.</param>
        /// <remarks>Should not throw exception if registration token not recognized.</remarks>
        void RemoveCallback(Guid callbackToken);

        /// <summary>
        /// Bound object.
        /// </summary>
        T BoundObject { get; }
    }
}