namespace Ipatov.DataBindings
{
    /// <summary>
    /// Object reference wrapper.
    /// </summary>
    /// <typeparam name="T">Type of reference.</typeparam>
    public interface IReferenceWrapper<T> where T : class 
    {
        /// <summary>
        /// Try to get reference.
        /// </summary>
        /// <param name="value">Reference value.</param>
        /// <returns>true, if reference is available.</returns>
        bool TryGetReference(out T value);

        /// <summary>
        /// Forcefully unbind reference.
        /// </summary>
        void Unbind();
    }
}