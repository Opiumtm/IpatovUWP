using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public sealed class DataBinding<T> : IDataBinding, IDataBindingEventCallback
    {
        private readonly IDataBindingEventSource _eventSource;

        private readonly Guid _callbackId;

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            _eventSource?.RemoveCallback(_callbackId);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="source">Value source.</param>
        /// <param name="target">Binding target.</param>
        /// <param name="eventSource">Event source.</param>
        public DataBinding(IDataBindingValueGetter<T> source, IDataBindingValueSetter<T> target, IDataBindingEventSource eventSource = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (target == null) throw new ArgumentNullException(nameof(target));
            Source = source;
            Target = target;
            _eventSource = eventSource;
            _callbackId = _eventSource?.AddCallback(this) ?? Guid.Empty;
        }

        public IDataBindingValueGetter<T> Source { get; }

        public IDataBindingValueSetter<T> Target { get; }

        /// <summary>
        /// Trigger data binding.
        /// </summary>
        public void Trigger()
        {
            try
            {
                Source.GetValue(v =>
                {
                    try
                    {
                        Target.SetValue(v, () => Success?.Invoke(this, EventArgs.Empty), ex => Error?.Invoke(this, ex));
                    }
                    catch (Exception ex)
                    {
                        Error?.Invoke(this, ex);
                    }
                }, ex => Error?.Invoke(this, ex));
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
        public void OnDataBindingEvent()
        {
            Trigger();
        }
    }
}