using System.Collections.Generic;

namespace Ipatov.Collections
{
    /// <summary>
    /// Tree dictionary node.
    /// </summary>
    internal struct TreeDictionaryNode<TKey>
        where TKey : struct
    {
        /// <summary>
        /// Key/
        /// </summary>
        public TKey Key;

        /// <summary>
        /// List index.
        /// </summary>
        public int? ListIndex;

        /// <summary>
        /// Level.
        /// </summary>
        public int Level;

        /// <summary>
        /// Child nodes.
        /// </summary>
        public List<TreeDictionaryNode<TKey>> Children;
    }
}