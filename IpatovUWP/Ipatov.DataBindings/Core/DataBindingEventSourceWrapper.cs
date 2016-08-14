using System;
using System.Threading;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Wrapper for dynamic data binding event source.
    /// </summary>
    /// <typeparam name="TParent">Parent object type.</typeparam>
    /// <typeparam name="T">Bound object type.</typeparam>
    public sealed class DataBindingEventSourceWrapper<TParent, T> : DataBindingEventSourceBase<T>, IDataBindingEventSourceWrapper<T>
    {
        private readonly IDataBindingEventSource<TParent> _parent;

        private readonly Func<TParent, T> _getBoundObject;

        private readonly Func<T, IDataBindingEventSource<T>> _getEventSource;

        private IDataBindingEventSource<T> _wrappedSource;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">Parent.</param>
        /// <param name="getBoundObject">Get bound object function.</param>
        /// <param name="getEventSource">Event source factory function.</param>
        public DataBindingEventSourceWrapper(IDataBindingEventSource<TParent> parent, Func<TParent, T> getBoundObject, Func<T, IDataBindingEventSource<T>> getEventSource)
        {
            if (getBoundObject == null) throw new ArgumentNullException(nameof(getBoundObject));
            if (getEventSource == null) throw new ArgumentNullException(nameof(getEventSource));
            _parent = parent;
            _getBoundObject = getBoundObject;
            _getEventSource = getEventSource;
            _parent?.AddCallback(new DataBindingDelegatedEventCallback(() => UpdateWrappedSource(false)));
            UpdateWrappedSource(true);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="getBoundObject">Get bound object function.</param>
        /// <param name="getEventSource">Event source factory function.</param>
        public DataBindingEventSourceWrapper(Func<TParent, T> getBoundObject, Func<T, IDataBindingEventSource<T>> getEventSource)
            :this(null, getBoundObject, getEventSource)
        {
        }

        /// <summary>
        /// Bound object.
        /// </summary>
        public override T BoundObject
        {
            get
            {
                var obj = Interlocked.CompareExchange(ref _wrappedSource, null, null);
                return obj != null ? obj.BoundObject : default(T);
            }
        } 

        private void UpdateWrappedSource(bool isFirst)
        {
            var newSource = _getEventSource(_getBoundObject(_parent != null ? _parent.BoundObject : default(TParent)));
            newSource?.AddCallback(new DataBindingDelegatedEventCallback(Trigger));
            var oldSource = Interlocked.Exchange(ref _wrappedSource, newSource);
            oldSource?.Dispose();
            if (!isFirst)
            {
                Trigger();
            }
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        protected override void OnDispose()
        {
            base.OnDispose();
            var source = Interlocked.CompareExchange(ref _wrappedSource, null, null);
            source?.Dispose();
            _parent?.Dispose();
        }
    }
}