using System;
using Windows.UI;

namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Text render style.
    /// </summary>
    public sealed class TextRenderStyle : ITextRenderStyle
    {
        /// <summary>
        /// Normal font face.
        /// </summary>
        public string FontFace { get; set; } = "Segoe UI";

        /// <summary>
        /// Monospace font face.
        /// </summary>
        public string FixedFontFace { get; set; } = "Courier New";

        /// <summary>
        /// Font size.
        /// </summary>
        public float FontSize { get; set; } = 14;

        /// <summary>
        /// Maximum number of lines.
        /// </summary>
        public int? MaxLines { get; } = null;

        /// <summary>
        /// Normal color.
        /// </summary>
        public Color NormalColor { get; set; } = Colors.Black;

        /// <summary>
        /// Quote color.
        /// </summary>
        public Color QuoteColor { get; set; } = Colors.Green;

        /// <summary>
        /// Spoiler background color.
        /// </summary>
        public Color SpoilerBackground { get; set; } = Colors.Gray;

        /// <summary>
        /// Spoiler foreground color.
        /// </summary>
        public Color SpoilerColor { get; set; } = Colors.Black;

        /// <summary>
        /// Link color.
        /// </summary>
        public Color LinkColor { get; set; } = Colors.Blue;

        /// <summary>
        /// Style changed.
        /// </summary>
        public event EventHandler StyleChanged;

        /// <summary>
        /// Trigger style changed.
        /// </summary>
        public void TriggerStyleChanged()
        {
            StyleChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}