using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Action binding.
    /// </summary>
    public sealed class ActionBinding : IDataBinding, IDataBindingEventCallback
    {
        private readonly IDataBindingEventSource _eventSource;

        private readonly Guid _callbackId;

        private readonly Action _action;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventSource">Event source.</param>
        /// <param name="action">Action.</param>
        public ActionBinding(IDataBindingEventSource eventSource, Action action)
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
                _action?.Invoke();
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