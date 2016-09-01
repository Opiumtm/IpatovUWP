namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Text render style.
    /// </summary>
    public interface ITextRenderStyle
    {
        /// <summary>
        /// Normal font face.
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

        /// <summary>
        /// Maximum number of lines.
        /// </summary>
        int? MaxLines { get; }

        /// <summary>
        /// Requested width.
        /// </summary>
        float Width { get; }
    }
}