using System;
using Windows.Foundation;
using Windows.Graphics.DirectX;
using Windows.UI.Composition;
using Windows.UI.Core;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;

namespace Ipatov.MarkupRender.Direct2D
{
    /// <summary>
    /// Composition UI Direct2D renderer.
    /// </summary>
    public sealed class Direct2DCompositionRenderer : Direct2DRendererBase
    {
        private readonly ICompositionGraphicsDeviceSource _deviceSource;

        private CompositionDrawingSurface _drawingSurface;

        private CompositionGraphicsDevice _drawingDevice;

        private Compositor _compositor;

        private SpriteVisual _drawingSurfaceVisual;

        /// <summary>
        /// Visual.
        /// </summary>
        public Visual Visual => _drawingSurfaceVisual;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="deviceSource">Composition device source.</param>
        public Direct2DCompositionRenderer(ICompositionGraphicsDeviceSource deviceSource)
        {
            if (deviceSource == null) throw new ArgumentNullException(nameof(deviceSource));
            _deviceSource = deviceSource;
            _deviceSource.Disposed += DeviceSourceOnDisposed;
            _compositor = _deviceSource.GetCompositor();
            _drawingDevice = _deviceSource.GetDevice();
            if (_compositor != null)
            {
                _drawingSurfaceVisual = _compositor.CreateSpriteVisual();
            }
            if (_drawingDevice != null)
            {
                _drawingDevice.RenderingDeviceReplaced += DrawingDeviceOnRenderingDeviceReplaced;
            }
            OnMapRefreshed(null);
        }

        private void DrawingDeviceOnRenderingDeviceReplaced(CompositionGraphicsDevice sender, RenderingDeviceReplacedEventArgs args)
        {
            var task = CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, Render);
        }

        /// <summary>
        /// Measure map refreshed.
        /// </summary>
        /// <param name="map">Measure map.</param>
        protected override void OnMapRefreshed(IRenderMeasureMap map)
        {
            base.OnMapRefreshed(map);
            if ( _drawingDevice != null)
            {
                Size bounds;
                if (map == null)
                {
                    bounds = new Size(10, 10);
                }
                else
                {
                    bounds = map.Bounds;
                }
                var surface = _drawingDevice.CreateDrawingSurface(bounds, DirectXPixelFormat.B8G8R8A8UIntNormalized, DirectXAlphaMode.Premultiplied);
                SetDrawingSurface(surface);
            }
            else
            {
                SetDrawingSurface(null);
            }
        }

        private void SetDrawingSurface(CompositionDrawingSurface surface)
        {
            _drawingSurface?.Dispose();
            _drawingSurface = surface;
            DisposeBrush();
            if (_drawingSurface != null && _drawingSurfaceVisual != null && _compositor != null)
            {
                _drawingSurfaceVisual.Brush = _compositor.CreateSurfaceBrush(_drawingSurface);
            }
        }


        private void DisposeBrush()
        {
            if (_drawingSurfaceVisual != null)
            {
                var brush = _drawingSurfaceVisual.Brush;
                _drawingSurfaceVisual.Brush = null;
                brush?.Dispose();
            }
        }

        private void DeviceSourceOnDisposed(object sender, EventArgs eventArgs)
        {
            DiscardAll();
        }

        private bool _isDiscarded;

        private void DiscardAll()
        {
            if (_isDiscarded)
            {
                return;
            }
            _isDiscarded = true;
            _compositor = null;
            if (_drawingDevice != null)
            {
                _drawingDevice.RenderingDeviceReplaced -= DrawingDeviceOnRenderingDeviceReplaced;
            }
            _drawingDevice = null;
            SetDrawingSurface(null);
            var o = _drawingSurfaceVisual;
            _drawingSurfaceVisual = null;
            o?.Dispose();
        }

        private bool _isDisposed;

        protected override void OnDispose()
        {
            base.OnDispose();
            DiscardAll();
            if (!_isDisposed)
            {
                _isDisposed = true;
                _deviceSource.Disposed -= DeviceSourceOnDisposed;
            }
        }

        /// <summary>
        /// Create drawing session.
        /// </summary>
        /// <returns>Drawing session.</returns>
        protected override CanvasDrawingSession CreateSession()
        {
            if (_drawingSurface == null)
            {
                return null;
            }
            return CanvasComposition.CreateDrawingSession(_drawingSurface);
        }
    }
}