using System.Collections.Generic;

namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Text attributes state.
    /// </summary>
    public interface ITextAttributeState
    {
        /// <summary>
        /// Get attributes snapshot.
        /// </summary>
        /// <returns>Snapshot.</returns>
        IReadOnlyDictionary<string, ITextAttribute> GetSnapshot();

        /// <summary>
        /// Add attribute.
        /// </summary>
        /// <param name="attribute">Attribute.</param>
        void Add(ITextAttribute attribute);

        /// <summary>
        /// Remove attribute.
        /// </summary>
        /// <param name="attributeName">Attribute name.</param>
        void Remove(string attributeName);

        /// <summary>
        /// Clear all attributes.
        /// </summary>
        void Clear();

        /// <summary>
        /// Clone attribute state.
        /// </summary>
        /// <returns>Exact clone.</returns>
        ITextAttributeState Clone();
    }
}