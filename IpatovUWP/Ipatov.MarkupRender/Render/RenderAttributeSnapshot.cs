using System.Collections.Generic;

namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Render attribute snapshot.
    /// </summary>
    public sealed class RenderAttributeSnapshot : IRenderAttributesSnapshot
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attributes">Attributes.</param>
        public RenderAttributeSnapshot(IReadOnlyDictionary<string, ITextAttribute> attributes)
        {
            Attributes = attributes;
        }

        /// <summary>
        /// Attributes.
        /// </summary>
        public IReadOnlyDictionary<string, ITextAttribute> Attributes { get; }
    }
}