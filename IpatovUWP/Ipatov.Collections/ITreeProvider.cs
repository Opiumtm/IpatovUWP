using System.Collections.Generic;

namespace Ipatov.Collections
{
    /// <summary>
    /// Tree value provider.
    /// </summary>
    /// <typeparam name="T">Base type.</typeparam>
    /// <typeparam name="TTreeKey">Tree key type.</typeparam>
    public interface ITreeProvider<in T, out TTreeKey> : IEqualityComparer<T>
        where TTreeKey : struct
    {
        /// <summary>
        /// Get count of key levels.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <returns>Count of levels.</returns>
        int GetLevelCount(T element);

        /// <summary>
        /// Get tree key of given level.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <param name="level">Level.</param>
        /// <returns>Tree key.</returns>
        TTreeKey GetKey(T element, int level);
    }
}