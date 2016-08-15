using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Bound action binding.
    /// </summary>
    /// <typeparam name="T">Bound object type.</typeparam>
    public sealed class BoundActionBinding<T> : IDataBinding, IDataBindingEventCallback
    {
        private readonly IDataBindingSource<T> _eventSource;

        private readonly Guid _callbackId;

        private readonly Action<T> _action;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventSource">Event source.</param>
        /// <param name="action">Action.</param>
        public BoundActionBinding(IDataBindingSource<T> eventSource, Action<T> action)
        {
            if (eventSource == null) throw new ArgumentNullException(nameof(eventSource));
            _action = action;
            _eventSource = eventSource;
            _callbackId = _eventSource.AddCallback(this);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            _eventSource.RemoveCallback(_callbackId);
            _eventSource.Dispose();
        }

        public void Trigger()
        {
            try
            {
                _eventSource.GetValue(OnValue, OnError);
            }
            catch (Exception ex)
            {
                Error?.Invoke(this, ex);
            }
        }

        private void OnError(Exception exception)
        {
            Error?.Invoke(this, exception);
        }

        private void OnValue(T obj)
        {
            try
            {
                _action?.Invoke(obj);
                Success?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Error?.Invoke(this, ex);
            }
        }

        /// <summary>
        /// Data binding error.
        /// </summary>
        public event EventHandler<Exception> Error;

        /// <summary>
        /// Data transfer success.
        /// </summary>
        public event EventHandler Success;

        /// <summary>
        /// Receive data binding event.
        /// </summary>
        void IDataBindingEventCallback.OnDataBindingEvent()
        {
            Trigger();
        }
    }
}