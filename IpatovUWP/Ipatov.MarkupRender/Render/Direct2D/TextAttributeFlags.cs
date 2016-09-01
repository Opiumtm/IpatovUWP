using System;

namespace Ipatov.MarkupRender.Direct2D
{
    /// <summary>
    /// Text attribute flags.
    /// </summary>
    [Flags]
    internal enum TextAttributeFlags : int
    {
        /// <summary>
        /// Bold.
        /// </summary>
        Bold = 0x0001,

        /// <summary>
        /// Italic.
        /// </summary>
        Italic = 0x0002,

        /// <summary>
        /// Monospace font.
        /// </summary>
        Fixed = 0x0004,

        /// <summary>
        /// Subscript.
        /// </summary>
        Subscript = 0x0008,

        /// <summary>
        /// Superscript.
        /// </summary>
        Superscript = 0x0010,

        /// <summary>
        /// Spoiler.
        /// </summary>
        Spoiler = 0x0020,

        /// <summary>
        /// Quote.
        /// </summary>
        Quote = 0x0040,

        /// <summary>
        /// Link.
        /// </summary>
        Link = 0x0080,

        /// <summary>
        /// Underline.
        /// </summary>
        Undeline = 0x0100,

        /// <summary>
        /// Overline.
        /// </summary>
        Overline = 0x0200,

        /// <summary>
        /// Strikethrough.
        /// </summary>
        Strikethrough = 0x0400
    }
}