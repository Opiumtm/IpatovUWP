namespace Ipatov.Collections
{
    /// <summary>
    /// Tree dictionary list node.
    /// </summary>
    /// <typeparam name="TKey">Type of key.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    internal struct TreeDictionaryListNode<TKey, TValue>
    {
        /// <summary>
        /// Key.
        /// </summary>
        public TKey Key;

        /// <summary>
        /// Value.
        /// </summary>
        public TValue Value;

        /// <summary>
        /// Node is assigned.
        /// </summary>
        public bool IsAssigned;
    }
}