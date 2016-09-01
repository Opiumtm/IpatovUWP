using System.Collections.Generic;

namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Render attributes snapshot.
    /// </summary>
    public interface IRenderAttributesSnapshot
    {
        /// <summary>
        /// Attributes.
        /// </summary>
        IReadOnlyDictionary<string, ITextAttribute> Attributes { get; }
    }
}