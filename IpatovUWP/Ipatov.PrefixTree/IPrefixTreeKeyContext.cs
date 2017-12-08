using System.Collections.Generic;

namespace Ipatov.DataStructures
{
    /// <summary>
    /// Prefix tree key context.
    /// </summary>
    public interface IPrefixTreeKeyContext<TKey, TKeyElement, out TKeyEnum, out TComparer>
        where TKeyEnum : IKeyElementsEnumerator<TKeyElement>
        where TComparer : IComparer<TKeyElement>
    {
        /// <summary>
        /// Key element comparer.
        /// </summary>
        TComparer KeyElementComparer { get; }

        /// <summary>
        /// Compose key from key elements.
        /// </summary>
        /// <param name="keyElements">Key elements.</param>
        /// <returns>Composed key.</returns>
        TKey ComposeKey(List<TKeyElement> keyElements);

        /// <summary>
        /// Enumerate key elements.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Key elements enumerator.</returns>
        TKeyEnum Enumerate(TKey key);
    }
}