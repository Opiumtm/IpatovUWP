using System.Collections.Generic;

namespace Ipatov.DataStructures
{
    /// <summary>
    /// Prefix tree for string keys.
    /// </summary>
    /// <typeparam name="TValue">Type of value.</typeparam>
    public class StringPrefixTreeDictionary<TValue> : PrefixTreeDictionary<string, char, TValue, StringTreeKeyContext.KeyEnumerator, StringTreeKeyContext.KeyComparer>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ignoreCase">Ignore char case.</param>
        public StringPrefixTreeDictionary(bool ignoreCase) : base(new StringTreeKeyContext(ignoreCase))
        {
        }
    }
}