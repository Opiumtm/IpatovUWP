using System.Collections.Generic;

namespace Ipatov.MarkupRender.Direct2D
{
    /// <summary>
    /// Text render measure map line.
    /// </summary>
    public interface IRenderMeasureMapLine
    {
        /// <summary>
        /// Get line measure mappings.
        /// </summary>
        /// <returns>Measure mapping.</returns>
        IReadOnlyList<RenderMeasureMapElement> GetMeasureMap();

        /// <summary>
        /// Line number starting from zero.
        /// </summary>
        int LineNumber { get; }

        /// <summary>
        /// Line height.
        /// </summary>
        double Height { get; }
    }
}