using System;
using System.Threading;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Wrapper for dynamic data binding event source.
    /// </summary>
    /// <typeparam name="TParent">Parent object type.</typeparam>
    /// <typeparam name="T">Bound object type.</typeparam>
    public sealed class DataBindingChildSource<TParent, T> : DataBindingEventSourceBase, IDataBindingEventSourceWrapper, IDataBindingSource<T>, IDataBindingStatusEvents
    {
        private readonly IDataBindingSource<TParent> _parent;

        private readonly Func<TParent, IDataBindingSource<T>> _getEventSource;

        private IDataBindingSource<T> _wrappedSource;

        private readonly Guid _callbackId;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">Parent.</param>
        /// <param name="getEventSource">Event source factory function.</param>
        public DataBindingChildSource(IDataBindingSource<TParent> parent, Func<TParent, IDataBindingSource<T>> getEventSource)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            if (getEventSource == null) throw new ArgumentNullException(nameof(getEventSource));
            _parent = parent;
            _getEventSource = getEventSource;
            _callbackId = _parent.AddCallback(new DataBindingDelegatedEventCallback(UpdateWrappedSource));
        }

        private void UpdateWrappedSource()
        {
            _parent.GetValue(OnParentValue, OnParentError);
        }

        private void OnParentError(Exception exception)
        {
            Error?.Invoke(this, exception);
        }

        private void OnParentValue(TParent obj)
        {
            var newSource = _getEventSource(obj);
            newSource?.AddCallback(new DataBindingDelegatedEventCallback(Trigger));
            var oldSource = Interlocked.Exchange(ref _wrappedSource, newSource);
            oldSource?.Dispose();
            Trigger();
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        protected override void OnDispose()
        {
            base.OnDispose();
            var source = Interlocked.CompareExchange(ref _wrappedSource, null, null);
            source?.Dispose();
            _parent?.RemoveCallback(_callbackId);
            _parent?.Dispose();
        }

        /// <summary>
        /// Get value.
        /// </summary>
        /// <param name="onValue">Action on value available.</param>
        /// <param name="onError">Action on value error.</param>
        public void GetValue(Action<T> onValue, Action<Exception> onError)
        {
            var source = Interlocked.CompareExchange(ref _wrappedSource, null, null);
            if (source == null)
            {
                onError?.Invoke(new InvalidOperationException("No data source currently available"));
            }
            else
            {
                try
                {
                    source.GetValue(onValue, onError);
                }
                catch (Exception ex)
                {
                    onError?.Invoke(ex);
                }
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
    }
}