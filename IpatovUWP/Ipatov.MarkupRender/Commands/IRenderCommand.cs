using System.Collections.Generic;

namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Render command.
    /// </summary>
    public interface IRenderCommand
    {
        /// <summary>
        /// Text attributes.
        /// </summary>
        IReadOnlyDictionary<string, ITextAttribute> Attributes { get; }

        /// <summary>
        /// Content.
        /// </summary>
        IRenderContent Content { get; }
    }
}