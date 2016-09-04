using Microsoft.Graphics.Canvas.Text;

namespace Ipatov.MarkupRender.Direct2D
{
    /// <summary>
    /// Render text layout result.
    /// </summary>
    public interface IRenderTextLayoutResult : IRenderDisposableWrapper<CanvasTextLayout>
    {
        /// <summary>
        /// Render style.
        /// </summary>
        ITextRenderStyle Style { get; }

        /// <summary>
        /// Plain text.
        /// </summary>
        string PlainText { get; }

        /// <summary>
        /// Render width.
        /// </summary>
        float Width { get; }
    }
}