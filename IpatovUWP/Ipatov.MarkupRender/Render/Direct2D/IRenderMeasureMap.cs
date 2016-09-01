using System.Collections.Generic;
using Windows.Foundation;

namespace Ipatov.MarkupRender.Direct2D
{
    public interface IRenderMeasureMap
    {
        /// <summary>
        /// Get measure map lines.
        /// </summary>
        /// <returns>Measure map lines.</returns>
        IReadOnlyList<IRenderMeasureMapLine> GetMeasureMapLines();

        /// <summary>
        /// Maximum number of lines (null - unlimited).
        /// </summary>
        int? MaxLines { get; }

        /// <summary>
        /// True, if maximum number of lines is exceeded.
        /// </summary>
        bool ExceedLines { get; }

        /// <summary>
        /// Bounds of text mapping.
        /// </summary>
        Size Bounds { get; }

        /// <summary>
        /// Render style.
        /// </summary>
        ITextRenderStyle Style { get; }
    }
}