using System.Collections.Generic;

namespace Ipatov.DataStructures
{
    /// <summary>
    /// Prefix tree for generic array.
    /// </summary>
    /// <typeparam name="TKeyElement">Type of array element.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    /// <typeparam name="TKeyComparer">Key element comparer.</typeparam>
    public class ArrayPrefixTreeDictionary<TKeyElement, TValue, TKeyComparer> : PrefixTreeDictionary<TKeyElement[], TKeyElement, TValue, ArrayTreeKeyContext<TKeyElement, TKeyComparer>.KeyEnumerator, TKeyComparer>
        where TKeyComparer : IComparer<TKeyElement>
    {
        public ArrayPrefixTreeDictionary(TKeyComparer elementComparer) : base(new ArrayTreeKeyContext<TKeyElement, TKeyComparer>(elementComparer))
        {
        }
    }
}