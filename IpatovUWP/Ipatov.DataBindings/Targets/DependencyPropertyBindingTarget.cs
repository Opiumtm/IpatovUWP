using System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Dependency property binding target.
    /// </summary>
    /// <typeparam name="T">Type of binded value.</typeparam>
    public sealed class DependencyPropertyBindingTarget<T> : IDataBindingTarget<T>
    {
        private readonly IReferenceWrapper<DependencyObject> _targetObject;

        private readonly DependencyProperty _targetProperty;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="targetObject">Target object.</param>
        /// <param name="targetProperty">Target property.</param>
        public DependencyPropertyBindingTarget(IReferenceWrapper<DependencyObject> targetObject, DependencyProperty targetProperty)
        {
            if (targetObject == null) throw new ArgumentNullException(nameof(targetObject));
            if (targetProperty == null) throw new ArgumentNullException(nameof(targetProperty));
            this._targetObject = targetObject;
            this._targetProperty = targetProperty;
        }

        /// <summary>
        /// Set binded value.
        /// </summary>
        /// <param name="value">Value.</param>
        public void SetValue(T value)
        {
            try
            {
                DependencyObject targetObject;
                if (_targetObject.TryGetReference(out targetObject))
                {
                    if (targetObject.Dispatcher.HasThreadAccess)
                    {
                        targetObject.SetValue(_targetProperty, value);
                        Success?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        targetObject.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            try
                            {
                                targetObject.SetValue(_targetProperty, value);
                                Success?.Invoke(this, EventArgs.Empty);
                            }
                            catch (Exception ex)
                            {
                                Error?.Invoke(this, ex);
                            }
                        });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    }
                }
            }
            catch (Exception ex)
            {
                Error?.Invoke(this, ex);
            }
        }

        /// <summary>
        /// Data binding success.
        /// </summary>
        public event EventHandler Success;

        /// <summary>
        /// Data binding error.
        /// </summary>
        public event EventHandler<Exception> Error;
    }
}