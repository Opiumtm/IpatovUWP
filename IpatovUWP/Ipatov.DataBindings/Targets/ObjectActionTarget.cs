using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Object action target.
    /// </summary>
    /// <typeparam name="T">Type of binded value.</typeparam>
    /// <typeparam name="TTarget">Type of object</typeparam>
    public sealed class ObjectActionTarget<T, TTarget> : IDataBindingTarget<T> where TTarget : class
    {
        private readonly IReferenceWrapper<TTarget> _targetObject;

        private readonly Action<TTarget, T> _action;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="targetObject">Target object.</param>
        /// <param name="action">Action.</param>
        public ObjectActionTarget(IReferenceWrapper<TTarget> targetObject, Action<TTarget, T> action)
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
                    _action(targetObject, value);
                    Success?.Invoke(this, EventArgs.Empty);
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