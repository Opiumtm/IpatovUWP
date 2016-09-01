namespace Ipatov.MarkupRender.Direct2D
{
    /// <summary>
    /// Measure mapper.
    /// </summary>
    public interface IRenderMeasureMapper
    {
        /// <summary>
        /// Map text layout.
        /// </summary>
        /// <param name="layout">Text layout.</param>
        /// <returns>Measure mapping.</returns>
        IRenderMeasureMap MapLayout(IRenderTextLayoutResult layout);
    }
}