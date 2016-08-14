using System;
using System.Threading;
using Windows.UI.Xaml;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Dependency property event source.
    /// </summary>
    /// <typeparam name="T">Bound object type</typeparam>
    public sealed class DataBindingDependencyPropertyChangedEventSource<T> : DataBindingWeakEventSourceBase<T>
        where T : DependencyObject
    {
        private readonly DependencyProperty _property;

        private readonly long _callbackId;

        private int _isDisposed;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly DependencyPropertyChangedCallback _callback;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="property">Dependency property.</param>
        public DataBindingDependencyPropertyChangedEventSource(T source, DependencyProperty property) : base(source)
        {
            if (_property == null) throw new ArgumentNullException(nameof(_property));
            _property = property;
            _callback = CreateCallback(new WeakReference<DataBindingDependencyPropertyChangedEventSource<T>>(this));
            _callbackId = source.RegisterPropertyChangedCallback(_property, _callback);
        }

        private static DependencyPropertyChangedCallback CreateCallback(
            WeakReference<DataBindingDependencyPropertyChangedEventSource<T>> handle)
        {
            return (sender, dp) =>
            {
                DataBindingDependencyPropertyChangedEventSource<T> obj;
                if (handle.TryGetTarget(out obj))
                {
                    obj.Trigger();
                }
            };
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            if (Interlocked.Exchange(ref _isDisposed, 1) == 0)
            {
                var source = BoundObject;
                if (source != null)
                {
                    source.UnregisterPropertyChangedCallback(_property, _callbackId);
                }
            }
        }
    }
}