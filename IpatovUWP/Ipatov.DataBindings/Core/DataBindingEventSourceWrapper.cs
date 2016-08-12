using System;
using System.Threading;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Wrapper for dynamic data binding event source.
    /// </summary>
    public sealed class DataBindingEventSourceWrapper : DataBindingEventSourceBase, IDataBindingEventSourceWrapper
    {
        private readonly IDataBindingEventSource _parent;

        private readonly Func<object, object> _getBoundObject;

        private readonly Func<object, IDataBindingEventSource> _getEventSource;

        private IDataBindingEventSource _wrappedSource;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parent">Parent.</param>
        /// <param name="getBoundObject">Get bound object function.</param>
        /// <param name="getEventSource">Event source factory function.</param>
        public DataBindingEventSourceWrapper(IDataBindingEventSource parent, Func<object, object> getBoundObject, Func<object, IDataBindingEventSource> getEventSource)
        {
            if (getBoundObject == null) throw new ArgumentNullException(nameof(getBoundObject));
            if (getEventSource == null) throw new ArgumentNullException(nameof(getEventSource));
            _parent = parent;
            _getBoundObject = getBoundObject;
            _getEventSource = getEventSource;
            _parent?.AddCallback(new DataBindingDelegatedEventCallback(UpdateWrappedSource));
            UpdateWrappedSource();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="getBoundObject">Get bound object function.</param>
        /// <param name="getEventSource">Event source factory function.</param>
        public DataBindingEventSourceWrapper(Func<object, object> getBoundObject, Func<object, IDataBindingEventSource> getEventSource)
            :this(null, getBoundObject, getEventSource)
        {
        }

        /// <summary>
        /// Bound object.
        /// </summary>
        public override object BoundObject
        {
            get
            {
                var obj = Interlocked.CompareExchange(ref _wrappedSource, null, null);
                return obj?.BoundObject;
            }
        } 

        private void UpdateWrappedSource()
        {
            var newSource = _getEventSource(_getBoundObject(_parent?.BoundObject));
            var oldSource = Interlocked.Exchange(ref _wrappedSource, newSource);
            newSource?.AddCallback(new DataBindingDelegatedEventCallback(Trigger));
            oldSource?.Dispose();
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