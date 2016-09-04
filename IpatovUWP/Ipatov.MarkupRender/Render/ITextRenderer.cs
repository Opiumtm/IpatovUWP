using System;
using Windows.Foundation;
using Ipatov.MarkupRender.Direct2D;

namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Text renderer.
    /// </summary>
    public interface ITextRenderer : IDisposable
    {
        /// <summary>
        /// Set render data.
        /// </summary>
        /// <param name="commandsSource">Command source.</param>
        /// <param name="style">Style.</param>
        /// <param name="width">Width.</param>
        void SetRenderData(IRenderCommandsSource commandsSource, ITextRenderStyle style, float width);

        /// <summary>
        /// Get text bounds.
        /// </summary>
        /// <returns>Text bounds.</returns>
        Size GetBounds();

        /// <summary>
        /// Render text.
        /// </summary>
        void Render();

        /// <summary>
        /// Find text at coordinates.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <returns>Render command at given coordinates or null if not found.</returns>
        IRenderCommand TextAt(Point point);

        /// <summary>
        /// Exceeds max lines.
        /// </summary>
        bool ExceedsLines { get; }

        /// <summary>
        /// Exceeds max lines value changed.
        /// </summary>
        event EventHandler ExceedsLinesChanged;
    }
}