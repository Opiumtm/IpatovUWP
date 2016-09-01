using System;
using System.Collections.Generic;

namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Render command.
    /// </summary>
    public sealed class RenderCommand : IRenderCommand
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="attributes">Text attributes.</param>
        /// <param name="content">Text content.</param>
        public RenderCommand(IReadOnlyDictionary<string, ITextAttribute> attributes, IRenderContent content)
        {
            if (attributes == null) throw new ArgumentNullException(nameof(attributes));
            if (content == null) throw new ArgumentNullException(nameof(content));
            Attributes = attributes;
            Content = content;
        }

        /// <summary>
        /// Text attributes.
        /// </summary>
        public IReadOnlyDictionary<string, ITextAttribute> Attributes { get; }

        /// <summary>
        /// Content.
        /// </summary>
        public IRenderContent Content { get; }
    }
}