using System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Dependency property binding source.
    /// </summary>
    /// <typeparam name="T">Type of binded value.</typeparam>
    public sealed class DependencyPropertyBindingSource<T> : IDataBindingSource<T>
    {
        private readonly long _eventToken;

        private readonly IReferenceWrapper<DependencyObject> _targetObject;

        private readonly DependencyProperty _targetProperty;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="targetObject">Targed object.</param>
        /// <param name="targetProperty">Target property.</param>
        public DependencyPropertyBindingSource(IReferenceWrapper<DependencyObject> targetObject, DependencyProperty targetProperty)
        {
            _targetObject = targetObject;
            _targetProperty = targetProperty;
            DependencyObject t;
            if (targetObject.TryGetReference(out t))
            {
                _eventToken = t.RegisterPropertyChangedCallback(targetProperty, CreateCallback(this));
            }
        }

        private static DependencyPropertyChangedCallback CreateCallback(DependencyPropertyBindingSource<T> source)
        {
            return (sender, dp) =>
            {
                source.Trigger();
            };
        }

        private void Trigger()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Выполняет определяемые приложением задачи, связанные с удалением, высвобождением или сбросом неуправляемых ресурсов.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            DependencyObject targetObject;
            if (_targetObject.TryGetReference(out targetObject))
            {
                targetObject.UnregisterPropertyChangedCallback(_targetProperty, _eventToken);
                _targetObject.Unbind();
            }
        }

        /// <summary>
        /// Get binded value.
        /// </summary>
        /// <returns>Value.</returns>
        public IDataPromise<T> GetValue()
        {
            try
            {
                DependencyObject targetObject;
                if (_targetObject.TryGetReference(out targetObject))
                {
                    if (targetObject.Dispatcher.HasThreadAccess)
                    {
                        return new ImmediateValue<T>((T)targetObject.GetValue(_targetProperty));
                    }
                    var promise = new DataPromise<T>();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    targetObject.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        try
                        {
                            promise.SetValue((T)targetObject.GetValue(_targetProperty));
                        }
                        catch (Exception ex)
                        {
                            promise.SetError(ex);
                        }
                    });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    return promise;
                }
                return null;
            }
            catch (Exception ex)
            {
                return new PromiseException<T>(ex);
            }
        }

        /// <summary>
        /// Binded value changed.
        /// </summary>
        public event EventHandler ValueChanged;
    }
}