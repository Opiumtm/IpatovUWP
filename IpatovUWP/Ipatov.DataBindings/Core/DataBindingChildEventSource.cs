using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Delegation of data binding event source for binding path support.
    /// </summary>
    public sealed class DataBindingChildEventSource : DataBindingEventSourceBase
    {
        private readonly IDataBindingEventSource _source;

        private readonly Func<IDataBindingEventSource> _getChildSoure;

        private ChildSourceInfo _currentChild;

        private readonly object _lock = new object();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="source">Parent event source.</param>
        /// <param name="getChildSoure">Child source factory.</param>
        public DataBindingChildEventSource(IDataBindingEventSource source, Func<IDataBindingEventSource> getChildSoure)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            _source = source;
            _getChildSoure = getChildSoure;
            _source.AddCallback(new DataBindingDelegatedEventCallback(SourceTriggered));
        }

        private void UpdateChildSource()
        {
            SetChildSource(_getChildSoure?.Invoke());
        }

        private void SetChildSource(IDataBindingEventSource childSource)
        {
            ChildSourceInfo cc, ccnew;
            lock (_lock)
            {
                cc = _currentChild;
                _currentChild = new ChildSourceInfo(childSource);
                ccnew = _currentChild;
            }
            cc?.Dispose();
            ccnew?.Bind(new DataBindingDelegatedEventCallback(OnBinding));
            Trigger();
        }

        private void OnBinding()
        {
            Trigger();
        }

        private void SourceTriggered()
        {
            UpdateChildSource();
        }

        /// <summary>
        /// Bound object.
        /// </summary>
        public override object BoundObject
        {
            get
            {
                ChildSourceInfo cc;
                lock (_lock)
                {
                    cc = _currentChild;
                }
                return cc?.BoundObject;
            }
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        protected override void OnDispose()
        {
            base.OnDispose();
            ChildSourceInfo cc;
            lock (_lock)
            {
                cc = _currentChild;
            }
            cc?.Dispose();
            _source.Dispose();
        }

        private sealed class ChildSourceInfo : IDisposable
        {
            public ChildSourceInfo(IDataBindingEventSource source)
            {
                _source = source;
            }

            private readonly IDataBindingEventSource _source;

            public void Dispose()
            {
                _source?.Dispose();
            }

            public void Bind(IDataBindingEventCallback callback)
            {
                _source?.AddCallback(callback);
            }

            public object BoundObject => _source?.BoundObject;
        }
    }
}