using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data promise.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    public sealed class DataPromise<T> : DataPromiseBase<T>
    {
        private bool _isCompleted;

        private Exception _error;

        private T _value;

        private bool _isValueSet;

        private readonly object _lock = new object();

        /// <summary>
        /// Continue data promise.
        /// </summary>
        /// <param name="callback">Data promise callback.</param>
        public override void Continue(IDataPromiseCallback<T> callback)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            bool isCompleted;
            lock (_lock)
            {
                isCompleted = _isCompleted;
                if (!isCompleted)
                {
                    RegisterCallback(callback);
                }
            }
            if (isCompleted)
            {
                TriggerCallback(callback);
            }
        }

        private void TriggerCallback(IDataPromiseCallback<T> callback)
        {
            if (callback == null)
            {
                return;
            }
            try
            {
                if (_error != null)
                {
                    callback.PromiseError(_error);
                }
                else if (_isValueSet)
                {
                    callback.ValueAvailable(_value);
                }
                else
                {
                    callback.PromiseError(new InvalidOperationException("Invalid promise state"));
                }
            }
            catch (Exception ex)
            {
                callback.PromiseError(ex);
            }
        }

        /// <summary>
        /// Set promise error.
        /// </summary>
        /// <param name="ex">Exception.</param>
        public void SetError(Exception ex)
        {
            if (ex == null) throw new ArgumentNullException(nameof(ex));
            IDataPromiseCallback<T>[] callbacks = null;
            lock (_lock)
            {
                if (!_isCompleted)
                {
                    _isCompleted = true;
                    _isValueSet = false;
                    _value = default(T);
                    _error = ex;
                    callbacks = ExtractAllCallbacks();
                }
            }
            if (callbacks != null)
            {
                foreach (var callback in callbacks)
                {
                    TriggerCallback(callback);
                }
            }
        }

        /// <summary>
        /// Set promise value.
        /// </summary>
        /// <param name="value">Promise value.</param>
        public void SetValue(T value)
        {
            IDataPromiseCallback<T>[] callbacks = null;
            lock (_lock)
            {
                if (!_isCompleted)
                {
                    _isCompleted = true;
                    _isValueSet = true;
                    _value = value;
                    _error = null;
                    callbacks = ExtractAllCallbacks();
                }
            }
            if (callbacks != null)
            {
                foreach (var callback in callbacks)
                {
                    TriggerCallback(callback);
                }
            }
        }
    }
}