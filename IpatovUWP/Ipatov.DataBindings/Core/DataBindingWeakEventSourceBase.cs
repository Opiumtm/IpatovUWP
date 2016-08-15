using System;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Event source base with weak reference to bound object.
    /// </summary>
    /// <typeparam name="T">Bound object type.</typeparam>
    public abstract class DataBindingWeakEventSourceBase<T> : DataBindingEventSourceBase where T : class
    {
        private readonly WeakReference<T> _handle;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="source">Source object.</param>
        protected DataBindingWeakEventSourceBase(T source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            _handle = new WeakReference<T>(source);
        }

        /// <summary>
        /// Weak reference to bound object.
        /// </summary>
        protected WeakReference<T> Handle => _handle;

        /// <summary>
        /// Bound object.
        /// </summary>
        protected T BoundObject
        {
            get
            {
                T obj;
                if (_handle.TryGetTarget(out obj))
                {
                    return obj;
                }
                return null;
            }
        }
    }
}