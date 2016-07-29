using System;
using System.Threading;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Weak reference to object.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    public sealed class WeakReferenceWrapper<T> : IReferenceWrapper<T> where T : class
    {
        private readonly WeakReference<T> _handle;

        private int _isUnbound;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reference">Source reference.</param>
        public WeakReferenceWrapper(T reference)
        {
            _handle = new WeakReference<T>(reference);
        }

        /// <summary>
        /// Try to get reference.
        /// </summary>
        /// <param name="value">Reference value.</param>
        /// <returns>true, if reference is available.</returns>
        public bool TryGetReference(out T value)
        {
            if (Interlocked.CompareExchange(ref _isUnbound, 0, 0) == 0)
            {
                return _handle.TryGetTarget(out value);
            }
            value = null;
            return false;
        }

        /// <summary>
        /// Forcefully unbind reference.
        /// </summary>
        public void Unbind()
        {
            Interlocked.Exchange(ref _isUnbound, 1);
        }
    }
}