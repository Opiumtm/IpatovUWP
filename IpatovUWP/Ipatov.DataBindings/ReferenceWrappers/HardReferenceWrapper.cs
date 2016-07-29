using System.Threading;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Weak reference to object.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    public sealed class HardReferenceWrapper<T> : IReferenceWrapper<T> where T :class
    {
        private T _reference;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reference">Source reference.</param>
        public HardReferenceWrapper(T reference)
        {
            _reference = reference;
        }

        /// <summary>
        /// Try to get reference.
        /// </summary>
        /// <param name="value">Reference value.</param>
        /// <returns>true, if reference is available.</returns>
        public bool TryGetReference(out T value)
        {
            var result = Interlocked.CompareExchange(ref _reference, null, null);
            value = result;
            return result != null;
        }

        /// <summary>
        /// Forcefully unbind reference.
        /// </summary>
        public void Unbind()
        {
            Interlocked.Exchange(ref _reference, null);
        }
    }
}