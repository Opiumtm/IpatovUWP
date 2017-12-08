using System.Collections.Generic;

namespace Ipatov.DataStructures
{
    /// <summary>
    /// Prefix tree for generic array.
    /// </summary>
    /// <typeparam name="TKeyElement">Type of array element.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    public class ArrayPrefixTreeDictionary<TKeyElement, TValue> : PrefixTreeDictionary<TKeyElement[], TKeyElement, TValue, ArrayTreeKeyContext<TKeyElement>.KeyEnumerator>
    {
        public ArrayPrefixTreeDictionary(IComparer<TKeyElement> elementComparer = null) : base(new ArrayTreeKeyContext<TKeyElement>(elementComparer))
        {
        }
    }
}