namespace Ipatov.DataStructures
{
    /// <summary>
    /// Key elements enumerator.
    /// </summary>
    /// <typeparam name="TKeyElement">Type of key element</typeparam>
    public interface IKeyElementsEnumerator<TKeyElement>
    {
        /// <summary>
        /// Get next key element.
        /// </summary>
        /// <returns>"Maybe" key element.</returns>
        bool GetNextKeyElement(out TKeyElement element);
    }
}