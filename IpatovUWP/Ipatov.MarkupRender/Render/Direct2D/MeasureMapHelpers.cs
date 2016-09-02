using Windows.Foundation;

namespace Ipatov.MarkupRender.Direct2D
{
    /// <summary>
    /// Measure map helper methods.
    /// </summary>
    public static class MeasureMapHelpers
    {
        /// <summary>
        /// Find text at coordinates.
        /// </summary>
        /// <param name="map">Measure map.</param>
        /// <param name="point">Point.</param>
        /// <returns>Render command at given coordinates or null if not found.</returns>
        public static IRenderCommand TextAt(this IRenderMeasureMap map, Point point)
        {
            if (map == null)
            {
                return null;
            }
            foreach (var line in map.GetMeasureMapLines())
            {
                if (map.MaxLines.HasValue)
                {
                    if (line.LineNumber >= map.MaxLines.Value)
                    {
                        break;
                    }
                }
                foreach (var element in line.GetMeasureMap())
                {
                    var r = new Rect(element.Placement, element.Size);
                    if (r.Contains(point))
                    {
                        return element.Command;
                    }
                }
            }
            return null;
        }

    }
}