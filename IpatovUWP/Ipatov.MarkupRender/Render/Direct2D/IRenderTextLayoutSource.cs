using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;

namespace Ipatov.MarkupRender.Direct2D
{
    /// <summary>
    /// Render text layout source.
    /// </summary>
    public interface IRenderTextLayoutSource
    {
        /// <summary>
        /// Create D2D text canvas layout.
        /// </summary>
        /// <param name="commandsSource">Text rendering commands source.</param>
        /// <param name="resourceCreator">Canvas resource creator.</param>
        /// <param name="style">Render style.</param>
        /// <param name="width">Requested width</param>
        /// <param name="height">Requested height</param>
        /// <returns>Cavvas text layout.</returns>
        IRenderDisposableWrapper<CanvasTextLayout> CreateLayout(IRenderCommandsSource commandsSource, ICanvasResourceCreator resourceCreator, ITextRenderStyle style, float width, float height = 10f);
    }
}