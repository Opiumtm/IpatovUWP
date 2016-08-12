using System;
using System.Collections.Concurrent;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding event source base.
    /// </summary>
    public abstract class DataBindingEventSourceBase : IDataBindingEventSource
    {
        private readonly ConcurrentDictionary<Guid, IDataBindingEventCallback> _callbacks = new ConcurrentDictionary<Guid, IDataBindingEventCallback>();

        /// <summary>
        /// Add data binding callback.
        /// </summary>
        /// <param name="callback">Callback.</param>
        /// <returns>Callback registration token.</returns>
        public Guid AddCallback(IDataBindingEventCallback callback)
        {
            if (callback == null)
            {
                return Guid.Empty;
            }
            var id = Guid.NewGuid();
            _callbacks.TryAdd(id, callback);
            return id;
        }

        /// <summary>
        /// Remove data binding callback.
        /// </summary>
        /// <param name="callbackToken">Callback registration token.</param>
        /// <remarks>Should not throw exception if registration token not recognized.</remarks>
        public virtual void RemoveCallback(Guid callbackToken)
        {
            IDataBindingEventCallback obj;
            _callbacks.TryRemove(callbackToken, out obj);
        }

        /// <summary>
        /// Bound object.
        /// </summary>
        public abstract object BoundObject { get; }

        /// <summary>
        /// Remove all registered callbacks.
        /// </summary>
        public virtual void RemoveAllCallbacks()
        {
            _callbacks.Clear();
        }

        /// <summary>
        /// Trigger event callbacks.
        /// </summary>
        protected void Trigger()
        {
            foreach (var kv in _callbacks)
            {
                try
                {
                    kv.Value.OnDataBindingEvent();
                }
                catch
                {
                    // ignore exceptions
                }
            }
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            _callbacks.Clear();
            OnDispose();
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        protected virtual void OnDispose()
        {            
        }
    }
}