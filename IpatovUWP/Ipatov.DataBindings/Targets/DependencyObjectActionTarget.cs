using System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Object action target.
    /// </summary>
    /// <typeparam name="T">Type of binded value.</typeparam>
    /// <typeparam name="TTarget">Type of object</typeparam>
    public sealed class DependencyObjectActionTarget<T, TTarget> : IDataBindingTarget<T> where TTarget : DependencyObject
    {
        private readonly IReferenceWrapper<TTarget> _targetObject;

        private readonly Action<TTarget, T> _action;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="targetObject">Target object.</param>
        /// <param name="action">Action.</param>
        public DependencyObjectActionTarget(IReferenceWrapper<TTarget> targetObject, Action<TTarget, T> action)
        {
            if (targetObject == null) throw new ArgumentNullException(nameof(targetObject));
            if (action == null) throw new ArgumentNullException(nameof(action));
            _targetObject = targetObject;
            _action = action;
        }

        /// <summary>
        /// Set binded value.
        /// </summary>
        /// <param name="value">Value.</param>
        public void SetValue(T value)
        {
            try
            {
                TTarget targetObject;
                if (_targetObject.TryGetReference(out targetObject))
                {
                    if (targetObject.Dispatcher.HasThreadAccess)
                    {
                        _action(targetObject, value);
                        Success?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        targetObject.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            try
                            {
                                _action(targetObject, value);
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