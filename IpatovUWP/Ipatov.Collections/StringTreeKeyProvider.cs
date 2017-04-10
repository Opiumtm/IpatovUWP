using System;

namespace Ipatov.Collections
{
    /// <summary>
    /// String tree key provider.
    /// </summary>
    public sealed class StringTreeKeyProvider : ITreeProvider<string, char>
    {
        /// <summary>
        /// Static instance.
        /// </summary>
        public static readonly StringTreeKeyProvider Intance = new StringTreeKeyProvider();

        /// <summary>
        /// Get count of key levels.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <returns>Count of levels.</returns>
        public int GetLevelCount(string element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            return element.Length;
        }

        /// <summary>
        /// Get tree key of given level.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <param name="level">Level.</param>
        /// <returns>Tree key.</returns>
        public char GetKey(string element, int level)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            return element[level];
        }

        /// <summary>Determines whether the specified objects are equal.</summary>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        /// <param name="x">The first object of type string to compare.</param>
        /// <param name="y">The second object of type string to compare.</param>
        public bool Equals(string x, string y)
        {
            return StringComparer.Ordinal.Equals(x, y);
        }

        /// <summary>Returns a hash code for the specified object.</summary>
        /// <returns>A hash code for the specified object.</returns>
        /// <param name="obj">The <see cref="T:System.Object" /> for which a hash code is to be returned.</param>
        /// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj" /> is a reference type and <paramref name="obj" /> is null.</exception>
        public int GetHashCode(string obj)
        {
            return StringComparer.Ordinal.GetHashCode(obj);
        }
    }
}