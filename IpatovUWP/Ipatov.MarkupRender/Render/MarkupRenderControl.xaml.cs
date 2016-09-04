using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Ipatov.MarkupRender.Direct2D;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Markup rendering control using Direct2D.
    /// </summary>
    public sealed partial class MarkupRenderControl : UserControl
    {
        private readonly IRenderMeasureMapper _mapper = new RenderMeasureMapper();

        private readonly IRenderTextLayoutSource _layoutSource = new RenderTextLayoutSource();

        private DisplayInformation _diHandle;

        private float _dpi = 96f;

        private IRenderMeasureMap _measureMap;

        private CanvasImageSource _imageSource;

        public MarkupRenderControl()
        {
            this.InitializeComponent();
            Loaded += MarkupRenderControl_OnLoaded;
            Unloaded += MarkupRenderControl_OnUnloaded;
            SizeChanged += MarkupRenderControl_OnSizeChanged;
        }

        private void DispatchAction(Action a)
        {
            var unwaitedTask = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    a?.Invoke();
                }
                catch
                {
                }
            });
        }

        private async Task<Tuple<IRenderMeasureMap, CanvasImageSource>> GetMapAndSwapChain()
        {
            if (Dispatcher.HasThreadAccess)
            {
                return new Tuple<IRenderMeasureMap, CanvasImageSource>(_measureMap, _imageSource);
            }
            else
            {
                Tuple<IRenderMeasureMap, CanvasImageSource> result = null;
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    result = new Tuple<IRenderMeasureMap, CanvasImageSource>(_measureMap, _imageSource);
                });
                return result;
            }
        }

        private async Task SetMap(IRenderMeasureMap map)
        {
            try
            {
                if (Dispatcher.HasThreadAccess)
                {
                    _measureMap = map;
                    InvalidateImageSource(map?.Bounds ?? new Size(0,0));
                    await RenderMap();
                }
                else
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        _measureMap = map;
                        InvalidateImageSource(map?.Bounds ?? new Size(0, 0));
                    });
                    await RenderMap();
                }
            }
            catch
            {
                // ignore
            }
        }

        private float GetActualWidth()
        {
            if (double.IsInfinity(ActualWidth))
            {
                return 0;
            }
            return (float)ActualWidth;
        }

        private async void InvalidateData(IMarkupRenderData renderData, float width, float dpi)
        {
            try
            {
                if (renderData?.Commands != null && renderData?.Style != null)
                {
                    if (width < 1f)
                    {
                        await SetMap(null);
                    }
                    else
                    {
                        IRenderMeasureMap map = null;
                        await Task.Factory.StartNew(() =>
                        {
                            var device = CanvasDevice.GetSharedDevice();
                            using (var t = new CanvasRenderTarget(device, (float)width, 10f, dpi))
                            {
                                using (var layout = _layoutSource.CreateLayout(renderData.Commands, t, renderData.Style, width))
                                {
                                    map = _mapper.MapLayout(layout);
                                }
                            }
                        });
                        await SetMap(map);
                    }
                }
                else
                {
                    await SetMap(null);
                }
            }
            catch
            {
                // ignore
            }
        }

        private readonly object _renderLock = new object();

        private async Task RenderMap()
        {
            try
            {
                var par = await GetMapAndSwapChain();
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (par?.Item1 != null && par?.Item2 != null && par.Item2.Size.Width > 15)
                    {
                        var swapChain = par.Item2;
                        var map = par.Item1;
                        lock (_renderLock)
                        {
                            using (var session = swapChain.CreateDrawingSession(Colors.Transparent))
                            {
                                using (var renderer = new Direct2DMapRenderer(session))
                                {
                                    session.Clear(Colors.Transparent);
                                    renderer.Render(map);
                                }
                            }
                        }
                    } else if (par?.Item2 != null)
                    {
                        var swapChain = par.Item2;
                        lock (_renderLock)
                        {
                            using (var session = swapChain.CreateDrawingSession(Colors.Transparent))
                            {
                                session.Clear(Colors.Transparent);
                            }
                        }
                    }
                });
            }
            catch
            {
                // ignore
            }
        }

        private Size? _mapSize;

        private void InvalidateImageSource(Size mapSize)
        {
            if (_mapSize != mapSize)
            {
                _mapSize = mapSize;
                if (mapSize.Height < 1 || mapSize.Width < 1)
                {
                    _imageSource = null;
                    ImageHost.Source = null;
                }
                else
                {
                    _imageSource = new CanvasImageSource(CanvasDevice.GetSharedDevice(), (float)mapSize.Width, (float)mapSize.Height, _dpi);
                    ImageHost.Source = _imageSource;
                    ImageHost.Width = mapSize.Width;
                    ImageHost.Height = mapSize.Height;
                }
            }
        }

        private void RenderDataChanged(IMarkupRenderData renderData)
        {
            DispatchAction(() => InvalidateData(renderData, GetActualWidth(), _dpi));
        }

        /// <summary>
        /// Markup data to render.
        /// </summary>
        public static readonly DependencyProperty RenderDataProperty = DependencyProperty.Register(
            "RenderData", typeof (IMarkupRenderData), typeof (MarkupRenderControl), new PropertyMetadata(default(IMarkupRenderData), RenderDataChangedCallback));

        /// <summary>
        /// Markup data to render.
        /// </summary>
        public IMarkupRenderData RenderData
        {
            get { return (IMarkupRenderData) GetValue(RenderDataProperty); }
            set { SetValue(RenderDataProperty, value); }
        }

        private static void RenderDataChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MarkupRenderControl)?.RenderDataChanged(e.NewValue as IMarkupRenderData);
        }

        private void MarkupRenderControl_OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_diHandle != null)
            {
                _diHandle.DpiChanged -= OnDpiChanged;
                _diHandle = null;
            }
            ImageHost.Source = null;
            _imageSource = null;
        }

        private void MarkupRenderControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            _diHandle = DisplayInformation.GetForCurrentView();
            _dpi = _diHandle?.LogicalDpi ?? 96f;
            if (_diHandle != null)
            {
                _diHandle.DpiChanged += OnDpiChanged;
            }
            InvalidateData(RenderData, GetActualWidth(), _dpi);
        }

        private void OnDpiChanged(DisplayInformation sender, object args)
        {
            DispatchAction(() =>
            {
                _dpi = _diHandle?.LogicalDpi ?? 96f;
                InvalidateData(RenderData, GetActualWidth(), _dpi);
            });
        }

        private float? _width;

        private void MarkupRenderControl_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            DispatchAction(() =>
            {
                var w = GetActualWidth();
                if (w != _width)
                {
                    _width = w;
                    InvalidateData(RenderData, w, _dpi);
                }
            });
        }
    }
}
