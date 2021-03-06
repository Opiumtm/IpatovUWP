﻿using System;
using Windows.UI;

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
        /// Normal color.
        /// </summary>
        Color NormalColor { get; }

        /// <summary>
        /// Quote color.
        /// </summary>
        Color QuoteColor { get; }

        /// <summary>
        /// Spoiler background color.
        /// </summary>
        Color SpoilerBackground { get; }

        /// <summary>
        /// Spoiler foreground color.
        /// </summary>
        Color SpoilerColor { get; }

        /// <summary>
        /// Link color.
        /// </summary>
        Color LinkColor { get; }

        /// <summary>
        /// Style changed.
        /// </summary>
        event EventHandler StyleChanged;
    }
}