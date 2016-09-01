using System;
using Windows.Foundation;
using Microsoft.Graphics.Canvas;

namespace Ipatov.MarkupRender.Direct2D
{
    /// <summary>
    /// Base class for Direct2D canvas renderer.
    /// </summary>
    public abstract class Direct2DRendererBase : ITextRenderer
    {
        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void OnDispose()
        {            
        }

        private IRenderMeasureMap _measureMap;

        private IRenderCommandsSource _commandsSource;

        private ITextRenderStyle _style;

        /// <summary>
        /// Set render data.
        /// </summary>
        /// <param name="commandsSource">Command source.</param>
        /// <param name="style">Style.</param>
        public void SetRenderData(IRenderCommandsSource commandsSource, ITextRenderStyle style)
        {
            if (commandsSource == null) throw new ArgumentNullException(nameof(commandsSource));
            if (style == null) throw new ArgumentNullException(nameof(style));
            _measureMap = null;
            _commandsSource = commandsSource;
            _style = style;
        }

        private bool EnsureMap()
        {
            if (_measureMap != null)
            {
                return true;
            }
            if (_commandsSource == null || _style == null)
            {
                return false;
            }
            var layoutSource = new RenderTextLayoutSource();
            var mapper = new RenderMeasureMapper();
            using (var session = CreateSession())
            {
                using (var layout = layoutSource.CreateLayout(_commandsSource, session, _style))
                {
                    _measureMap = mapper.MapLayout(layout);
                    return true;
                }
            }
        }

        /// <summary>
        /// Get text bounds.
        /// </summary>
        /// <returns>Text bounds.</returns>
        public Size GetBounds()
        {
            if (EnsureMap())
            {
                return _measureMap.Bounds;
            }
            return Size.Empty;
        }

        /// <summary>
        /// Render text.
        /// </summary>
        public void Render()
        {
            EnsureMap();
            using (var mapRenderer = new Direct2DMapRenderer(CreateSession()))
            {
                mapRenderer.Render(_measureMap);
            }
        }

        /// <summary>
        /// Find text at coordinates.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <returns>Render command at given coordinates or null if not found.</returns>
        public IRenderCommand TextAt(Point point)
        {
            EnsureMap();
            if (_measureMap == null)
            {
                return null;
            }
            foreach (var line in _measureMap.GetMeasureMapLines())
            {
                if (_measureMap.MaxLines.HasValue)
                {
                    if (line.LineNumber >= _measureMap.MaxLines.Value)
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

        /// <summary>
        /// Create drawing session.
        /// </summary>
        /// <returns>Drawing session.</returns>
        protected abstract CanvasDrawingSession CreateSession();
    }
}