using System.Collections.Generic;

namespace Ipatov.DataStructures
{
    /// <summary>
    /// Prefix tree for string keys.
    /// </summary>
    /// <typeparam name="TValue">Type of value.</typeparam>
    public class StringPrefixTreeDictionary<TValue> : PrefixTreeDictionary<string, char, TValue, StringTreeKeyContext.KeyEnumerator>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="charComparer">Char comparer.</param>
        public StringPrefixTreeDictionary(IComparer<char> charComparer = null) : base(new StringTreeKeyContext(charComparer))
        {
        }
    }
}