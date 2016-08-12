using System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Value wrapper with access to source object on UI thread.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public sealed class DataBindingOnUiThreadValueAccessWrapper<T> : IDataBindingAccessor<T>
    {
        private readonly IDataBindingAccessor<T> _source;

        private readonly CoreDispatcher _dispatcher;

        private readonly CoreDispatcherPriority _priority;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="source">Wrapped data accessor.</param>
        /// <param name="dispatcher">UI thread dispatcher.</param>
        /// <param name="priority">Dispatcher priority.</param>
        public DataBindingOnUiThreadValueAccessWrapper(IDataBindingAccessor<T> source, CoreDispatcher dispatcher = null, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            _source = source;
            _dispatcher = dispatcher ?? Window.Current?.Dispatcher;
            _priority = priority;
        }

        /// <summary>
        /// Get value.
        /// </summary>
        /// <param name="onValue">Action on value available.</param>
        /// <param name="onError">Action on value error.</param>
        public void GetValue(Action<T> onValue, Action<Exception> onError)
        {
            try
            {
                if (_dispatcher == null)
                {
                    onError(new InvalidOperationException("Thread access dispatcher is not available"));
                    return;
                }
                if (_dispatcher.HasThreadAccess)
                {
                    _source.GetValue(onValue, onError);
                }
                else
                {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    _dispatcher.RunAsync(_priority, () =>
                    {
                        try
                        {
                            _source.GetValue(onValue, onError);
                        }
                        catch (Exception ex)
                        {
                            onError?.Invoke(ex);
                        }
                    });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }
        }

        /// <summary>
        /// Set value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="onSuccess">Action on value setter success.</param>
        /// <param name="onError">Action on error.</param>
        public void SetValue(T value, Action onSuccess, Action<Exception> onError)
        {
            try
            {
                if (_dispatcher == null)
                {
                    onError(new InvalidOperationException("Thread access dispatcher is not available"));
                    return;
                }
                if (_dispatcher.HasThreadAccess)
                {
                    _source.SetValue(value, onSuccess, onError);
                }
                else
                {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    _dispatcher.RunAsync(_priority, () =>
                    {
                        try
                        {
                            _source.SetValue(value, onSuccess, onError);
                        }
                        catch (Exception ex)
                        {
                            onError?.Invoke(ex);
                        }
                    });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }
        }
    }
}