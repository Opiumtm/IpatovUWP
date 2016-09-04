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

        private float _width = 100f;

        /// <summary>
        /// Set render data.
        /// </summary>
        /// <param name="commandsSource">Command source.</param>
        /// <param name="style">Style.</param>
        /// <param name="width">Width.</param>
        public void SetRenderData(IRenderCommandsSource commandsSource, ITextRenderStyle style, float width)
        {
            if (commandsSource == null) throw new ArgumentNullException(nameof(commandsSource));
            if (style == null) throw new ArgumentNullException(nameof(style));
            _measureMap = null;
            _commandsSource = commandsSource;
            _style = style;
            _width = width;
        }

        /// <summary>
        /// Measure map refreshed.
        /// </summary>
        /// <param name="map">Measure map.</param>
        protected virtual void OnMapRefreshed(IRenderMeasureMap map)
        {
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
            var session = CreateSession();
            if (session == null)
            {
                return false;
            }
            using (session)
            {
                using (var layout = layoutSource.CreateLayout(_commandsSource, session, _style, _width))
                {
                    _measureMap = mapper.MapLayout(layout);
                    OnMapRefreshed(_measureMap);
                    ExceedsLinesChanged?.Invoke(this, EventArgs.Empty);
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
            if (EnsureMap())
            {
                var session = CreateSession();
                if (session != null)
                {
                    using (var mapRenderer = new Direct2DMapRenderer(session))
                    {
                        mapRenderer.Render(_measureMap);
                    }
                }
            }
        }

        /// <summary>
        /// Find text at coordinates.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <returns>Render command at given coordinates or null if not found.</returns>
        public IRenderCommand TextAt(Point point)
        {
            if (!EnsureMap())
            {
                return null;
            }
            return _measureMap.TextAt(point);
        }

        /// <summary>
        /// Exceeds max lines.
        /// </summary>
        public bool ExceedsLines => _measureMap?.ExceedLines ?? false;

        /// <summary>
        /// Exceeds max lines value changed.
        /// </summary>
        public event EventHandler ExceedsLinesChanged;

        /// <summary>
        /// Create drawing session.
        /// </summary>
        /// <returns>Drawing session.</returns>
        protected abstract CanvasDrawingSession CreateSession();
    }
}