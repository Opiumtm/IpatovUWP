using System;
using System.ComponentModel;
using System.Threading;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// INotifyPropertyChanged event source.
    /// </summary>
    /// <typeparam name="T">Bound object type</typeparam>
    public sealed class DataBindingPropertyChangedEventSource<T> : DataBindingWeakEventSourceBase<T> where T : class, INotifyPropertyChanged
    {
        private readonly string _property;

        private readonly PropertyChangedEventHandler _handler;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="property">Property name.</param>
        public DataBindingPropertyChangedEventSource(T source, string property)
            :base(source)
        {
            _property = property;
            _handler = CreateHandler(new WeakReference<DataBindingPropertyChangedEventSource<T>>(this));
            source.PropertyChanged += _handler;
        }

        private static PropertyChangedEventHandler CreateHandler(WeakReference<DataBindingPropertyChangedEventSource<T>> handle)
        {
            return (sender, e) =>
            {
                DataBindingPropertyChangedEventSource<T> obj;
                if (handle.TryGetTarget(out obj))
                {
                    if (obj._property == e.PropertyName || e.PropertyName == null)
                    {
                        obj.Trigger();
                    }
                }
            };
        }

        private int _isDisposed;

        protected override void OnDispose()
        {
            base.OnDispose();
            if (Interlocked.Exchange(ref _isDisposed, 1) == 0)
            {
                var source = BoundObject;
                if (source != null)
                {
                    source.PropertyChanged -= _handler;
                }
            }
        }
    }
}