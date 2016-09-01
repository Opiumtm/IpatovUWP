namespace Ipatov.MarkupRender.Direct2D
{
    /// <summary>
    /// Mapping helper argument.
    /// </summary>
    internal interface IMappingHelperArg
    {
        /// <summary>
        /// Index.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// String to render.
        /// </summary>
        string RenderString { get; }

        /// <summary>
        /// Attribute flags.
        /// </summary>
        TextAttributeFlags Flags { get; }

        /// <summary>
        /// Command.
        /// </summary>
        object Command { get; }

        /// <summary>
        /// Font face.
        /// </summary>
        string FontFace { get; }

        /// <summary>
        /// Monospace font face.
        /// </summary>
        string FixedFontFace { get; }

        /// <summary>
        /// Font size.
        /// </summary>
        float FontSize { get; }
    }
}