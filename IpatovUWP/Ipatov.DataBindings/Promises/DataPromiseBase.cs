using System;
using System.Collections.Generic;
using System.Linq;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data promise base class.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    public abstract class DataPromiseBase<T> : IDataPromise<T>
    {
        private readonly Dictionary<Guid, IDataPromiseCallback<T>> _callbacks = new Dictionary<Guid, IDataPromiseCallback<T>>();

        public abstract void Continue(IDataPromiseCallback<T> callback);

        /// <summary>
        /// Register data callback.
        /// </summary>
        /// <param name="callback">Data callback.</param>
        /// <returns>Registration token.</returns>
        protected Guid RegisterCallback(IDataPromiseCallback<T> callback)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            var token = Guid.NewGuid();
            _callbacks.Add(token, callback);
            return token;
        }

        /// <summary>
        /// Extract data callback.
        /// </summary>
        /// <param name="token">Registration token.</param>
        /// <returns>Callback.</returns>
        protected IDataPromiseCallback<T> ExtractCallback(Guid token)
        {
            if (_callbacks.ContainsKey(token))
            {
                return null;
            }
            var callback = _callbacks[token];
            _callbacks.Remove(token);
            return callback;
        }

        /// <summary>
        /// Extract all data callbacks.
        /// </summary>
        /// <returns>Callbacks.</returns>
        protected IDataPromiseCallback<T>[] ExtractAllCallbacks()
        {
            var result = _callbacks.Values.ToArray();
            _callbacks.Clear();
            return result;
        }
    }
}