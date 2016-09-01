using System;
using System.Collections.Generic;
using Windows.UI;
using Microsoft.Graphics.Canvas;

namespace Ipatov.MarkupRender.Direct2D
{
    /// <summary>
    /// Direct2D text renderer.
    /// </summary>
    public sealed class Direct2DMapRenderer : IMeasureMapRenderer, IDisposable
    {
        private readonly CanvasDrawingSession _drawingSession;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="drawingSession">Drawing session.</param>
        public Direct2DMapRenderer(CanvasDrawingSession drawingSession)
        {
            if (drawingSession == null) throw new ArgumentNullException(nameof(drawingSession));
            _drawingSession = drawingSession;
        }

        /// <summary>
        /// Render measured map.
        /// </summary>
        /// <param name="renderMap">Map.</param>
        public void Render(IRenderMeasureMap renderMap)
        {
            _drawingSession.Clear(Colors.Transparent);
            if (renderMap != null)
            {
                DoRender(renderMap);
            }
        }

        private void DoRender(IRenderMeasureMap map)
        {
            foreach (var line in map.GetMeasureMapLines())
            {
                if (map.MaxLines.HasValue)
                {
                    if (line.LineNumber >= map.MaxLines.Value)
                    {
                        break;
                    }
                }
                foreach (var el in line.GetMeasureMap())
                {
                    RenderElement(el);
                }
            }
        }

        private void RenderElement(RenderMeasureMapElement element)
        {
        }

        /// <summary>
        /// Dispose object.
        /// </summary>
        public void Dispose()
        {
            _drawingSession.Dispose();
        }
    }
}