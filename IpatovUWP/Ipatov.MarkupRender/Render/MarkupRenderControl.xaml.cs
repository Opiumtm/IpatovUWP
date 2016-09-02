using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Ipatov.MarkupRender
{
    /// <summary>
    /// Markup rendering control using Direct2D.
    /// </summary>
    public sealed partial class MarkupRenderControl : UserControl
    {
        private readonly IRenderMeasureMapper _mapper = new RenderMeasureMapper();

        private DisplayInformation _diHandle;

        private float _dpi = 96f;

        private IRenderMeasureMap _measureMap;

        private CanvasSwapChain _swapChain;

        public MarkupRenderControl()
        {
            this.InitializeComponent();
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

        private void InvalidateData(IMarkupRenderData renderData)
        {
        }

        private void RenderMap()
        {
            if (_measureMap != null && _swapChain != null)
            {
                using (var session = _swapChain.CreateDrawingSession(Colors.Transparent))
                {
                    
                }
            }
        }

        private void InvalidateSwapChain()
        {
            if (_swapChain != null)
            {
                _swapChain.Dispose();
                _swapChain = null;
            }
            _swapChain = new CanvasSwapChain(CanvasDevice.GetSharedDevice(), (float)ActualWidth, (float)ActualHeight, _dpi);
            SwapChainPanel.SwapChain = _swapChain;
        }

        private void RenderDataChanged(IMarkupRenderData renderData)
        {
            DispatchAction(() => InvalidateData(renderData));
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (_measureMap == null)
            {
                return new Size(10, 10);
            }
            return _measureMap.Bounds;
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
            SwapChainPanel.RemoveFromVisualTree();
            SwapChainPanel = null;
            if (_diHandle != null)
            {
                _diHandle.DpiChanged -= OnDpiChanged;
                _diHandle = null;
            }
            if (_swapChain != null)
            {
                _swapChain.Dispose();
                _swapChain = null;
            }
        }

        private void MarkupRenderControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            _diHandle = DisplayInformation.GetForCurrentView();
            _dpi = _diHandle?.LogicalDpi ?? 96f;
            if (_diHandle != null)
            {
                _diHandle.DpiChanged += OnDpiChanged;
            }
            InvalidateSwapChain();
        }

        private void OnDpiChanged(DisplayInformation sender, object args)
        {
            DispatchAction(() =>
            {
                _dpi = _diHandle?.LogicalDpi ?? 96f;
                InvalidateSwapChain();
            });
        }

        private void MarkupRenderControl_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            InvalidateSwapChain();
        }
    }
}
