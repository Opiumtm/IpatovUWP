using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding source.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    /// <typeparam name="TSrc">Bound object type.</typeparam>
    public sealed class DataBindingSource<T, TSrc> : IDataBindingSource<T, TSrc>
    {
        private readonly IDataBindingValueGetter<T> _valueGetter;

        private readonly IDataBindingEventSource<TSrc> _eventSource;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="valueGetter">Value getter.</param>
        /// <param name="eventSource">Event source.</param>
        public DataBindingSource(IDataBindingValueGetter<T> valueGetter, IDataBindingEventSource<TSrc> eventSource = null)
        {
            if (valueGetter == null) throw new ArgumentNullException(nameof(valueGetter));
            _valueGetter = valueGetter;
            _eventSource = eventSource;
        }

        /// <summary>
        /// Get value.
        /// </summary>
        /// <param name="onValue">Action on value available.</param>
        /// <param name="onError">Action on value error.</param>
        public void GetValue(Action<T> onValue, Action<Exception> onError)
        {
            _valueGetter.GetValue(onValue, onError);
        }

        /// <summary>
        /// Add data binding callback.
        /// </summary>
        /// <param name="callback">Callback.</param>
        /// <returns>Callback registration token.</returns>
        public Guid AddCallback(IDataBindingEventCallback callback)
        {
            return _eventSource?.AddCallback(callback) ?? Guid.Empty;
        }

        /// <summary>
        /// Remove data binding callback.
        /// </summary>
        /// <param name="callbackToken">Callback registration token.</param>
        /// <remarks>Should not throw exception if registration token not recognized.</remarks>
        public void RemoveCallback(Guid callbackToken)
        {
            _eventSource?.RemoveCallback(callbackToken);
        }

        /// <summary>
        /// Bound object.
        /// </summary>
        public TSrc BoundObject => _eventSource != null ? _eventSource.BoundObject : default(TSrc);

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            _eventSource?.Dispose();
        }
    }
}