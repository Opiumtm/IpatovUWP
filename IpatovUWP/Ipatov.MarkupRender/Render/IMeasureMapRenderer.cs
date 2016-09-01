using Ipatov.MarkupRender.Direct2D;

namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Measure map renderer.
    /// </summary>
    public interface IMeasureMapRenderer
    {
        /// <summary>
        /// Render measured map.
        /// </summary>
        /// <param name="renderMap">Map.</param>
        void Render(IRenderMeasureMap renderMap);
    }
}