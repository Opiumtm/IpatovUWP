using Windows.Foundation;

namespace Ipatov.MarkupRender.Direct2D
{
    /// <summary>
    /// Measured render element.
    /// </summary>
    public struct RenderMeasureMapElement
    {
        /// <summary>
        /// Render command.
        /// </summary>
        public IRenderCommand Command;

        /// <summary>
        /// Text placement.
        /// </summary>
        public Point Placement;

        /// <summary>
        /// Text size.
        /// </summary>
        public Size Size;
    }
}