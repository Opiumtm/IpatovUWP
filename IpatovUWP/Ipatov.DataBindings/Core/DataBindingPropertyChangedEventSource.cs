using System;
using System.ComponentModel;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// INotifyPropertyChanged event source.
    /// </summary>
    public sealed class DataBindingPropertyChangedEventSource : DataBindingEventSourceBase
    {
        private readonly WeakReference<INotifyPropertyChanged> _handle;

        private readonly string _property;

        private readonly PropertyChangedEventHandler _handler;

        public DataBindingPropertyChangedEventSource(INotifyPropertyChanged source, string property)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            _handle = new WeakReference<INotifyPropertyChanged>(source);
            _property = property;
            _handler = CreateHandler(new WeakReference<DataBindingPropertyChangedEventSource>(this));
            source.PropertyChanged += _handler;
        }

        private static PropertyChangedEventHandler CreateHandler(WeakReference<DataBindingPropertyChangedEventSource> handle)
        {
            return (sender, e) =>
            {
                DataBindingPropertyChangedEventSource obj;
                if (handle.TryGetTarget(out obj))
                {
                    if (obj._property == e.PropertyName || e.PropertyName == null)
                    {
                        obj.Trigger();
                    }
                }
            };
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            var source = BoundObject as INotifyPropertyChanged;
            if (source != null)
            {
                source.PropertyChanged -= _handler;
            }
        }

        /// <summary>
        /// Bound object.
        /// </summary>
        public override object BoundObject
        {
            get
            {
                INotifyPropertyChanged obj;
                if (_handle.TryGetTarget(out obj))
                {
                    return obj;
                }
                return null;
            }
        }
    }
}