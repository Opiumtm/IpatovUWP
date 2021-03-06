﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input;
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

        private IRenderMeasureMap _measureMap;

        private readonly bool _isResumedSet;


        public MarkupRenderControl()
        {
            this.InitializeComponent();
            Loaded += MarkupRenderControl_OnLoaded;
            Unloaded += MarkupRenderControl_OnUnloaded;
            SizeChanged += MarkupRenderControl_OnSizeChanged;
            if (Application.Current != null)
            {
                if (Windows.Foundation.Metadata.ApiInformation.IsEventPresent("Windows.UI.Xaml.Application", nameof(Application.LeavingBackground)))
                {
                    Application.Current.LeavingBackground += CurrentOnLeavingBackground;
                }
                else
                {
                    Application.Current.Resuming += CurrentOnResuming;
                }
                _isResumedSet = true;
            }
        }

        private void CurrentOnResuming(object sender, object o)
        {
            var unwaitedTask = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, WaitAndInvalidate);
        }

        private void CurrentOnLeavingBackground(object sender, LeavingBackgroundEventArgs leavingBackgroundEventArgs)
        {
            var unwaitedTask = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, WaitAndInvalidate);
        }

        private async void WaitAndInvalidate()
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(1.5));
                InvalidateImageSource(_mapSize ?? new Size(0, 0), true, true);
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }
        }

        private void DispatchAction(Action a)
        {
            if (Dispatcher.HasThreadAccess)
            {
                a?.Invoke();
            }
            else
            {
                var unwaitedTask = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    try
                    {
                        a?.Invoke();
                    }
                    catch
                    {
                        // ignore
                    }
                });

            }
        }

        private void SetMap(IRenderMeasureMap map)
        {
            try
            {
                _measureMap = map;
                InvalidateImageSource(map?.Bounds ?? new Size(0,0), false, false);
                RenderHost?.Invalidate();
                var unwaitedTask = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    try
                    {
                        ExceedsLines = map?.ExceedLines ?? false;
                    }
                    catch
                    {
                        // ignore
                    }
                });
            }
            catch
            {
                // ignore
            }
        }

        private float GetActualWidth()
        {
            if (double.IsInfinity(ActualWidth) || double.IsNaN(ActualWidth))
            {
                return 0;
            }
            return (float)ActualWidth;
        }

        private void InvalidateData(IMarkupRenderData renderData, float width)
        {
            try
            {
                if (renderData?.Commands != null && renderData?.Style != null)
                {
                    if (width < 1f)
                    {
                        SetMap(null);
                    }
                    else
                    {
                        IRenderMeasureMap map = null;
                        var device = CanvasDevice.GetSharedDevice();
                        var di = DisplayInformation.GetForCurrentView();
                        using (var t = new CanvasRenderTarget(device, (float)width, 10f, di.LogicalDpi))
                        {
                            using (var layout = _layoutSource.CreateLayout(renderData.Commands, t, renderData.Style, width))
                            {
                                map = _mapper.MapLayout(layout);
                            }
                        }
                        SetMap(map);
                    }
                }
                else
                {
                    SetMap(null);
                }
            }
            catch
            {
                // ignore
            }
        }

        private void RenderMap(CanvasDrawingSession session, Rect r)
        {
            using (var renderer = new Direct2DMapRenderer(session, r) { DisposeSession = false, ClearOnRenderColor = Colors.Transparent })
            {
                renderer.Render(_measureMap);
            }
        }

        private Size? _mapSize;

        private void InvalidateImageSource(Size mapSize, bool force, bool invalidate)
        {
            if (_mapSize != mapSize || force)
            {
                _mapSize = mapSize;
                if (RenderHost != null)
                {
                    RenderHost.Width = Math.Max(10, mapSize.Width);
                    RenderHost.Height = Math.Max(10, mapSize.Height);
                    if (invalidate)
                    {
                        RenderHost.Invalidate();
                    }
                }
            }
        }

        private bool _isUnloaded;

        private void RenderDataChanged(IMarkupRenderData renderData, IMarkupRenderData oldData)
        {
            DispatchAction(() =>
            {
                if (oldData != null)
                {
                    oldData.PropertyChanged -= RenderDataOnPropertyChanged;
                }
                if (renderData != null)
                {
                    renderData.PropertyChanged += RenderDataOnPropertyChanged;
                }
                StyleObjectChanged(renderData?.Style);
                if (!_isUnloaded)
                {
                    InvalidateData(renderData, GetActualWidth());
                }
            });
        }

        private ITextRenderStyle _currentStyle;

        private void StyleObjectChanged(ITextRenderStyle style)
        {
            if (_currentStyle != null)
            {
                _currentStyle.StyleChanged -= StyleOnStyleChanged;
            }
            if (style != null)
            {
                style.StyleChanged += StyleOnStyleChanged;
            }
            _currentStyle = style;
        }

        private void RenderDataOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DispatchAction(() =>
            {
                if (object.ReferenceEquals(sender, RenderData))
                {
                    if (e.PropertyName == nameof(IMarkupRenderData.Commands))
                    {
                        InvalidateData(RenderData, GetActualWidth());
                    }
                    if (e.PropertyName == nameof(IMarkupRenderData.Style))
                    {
                        StyleObjectChanged(RenderData?.Style);
                        InvalidateData(RenderData, GetActualWidth());
                    }
                }
            });
        }

        private void StyleOnStyleChanged(object sender, EventArgs eventArgs)
        {
            DispatchAction(() =>
            {
                InvalidateData(RenderData, GetActualWidth());
            });
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
            (d as MarkupRenderControl)?.RenderDataChanged(e.NewValue as IMarkupRenderData, e.OldValue as IMarkupRenderData);
        }

        /// <summary>
        /// Exceeds maximum lines.
        /// </summary>
        public static readonly DependencyProperty ExceedsLinesProperty = DependencyProperty.Register(
            "ExceedsLines", typeof (bool), typeof (MarkupRenderControl), new PropertyMetadata(default(bool), ExceedsLinesPropertyChangedCallback));

        /// <summary>
        /// Exceeds maximum lines.
        /// </summary>
        public bool ExceedsLines
        {
            get { return (bool) GetValue(ExceedsLinesProperty); }
            set { SetValue(ExceedsLinesProperty, value); }
        }

        private static void ExceedsLinesPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MarkupRenderControl)?.ExceedsLinesChanged?.Invoke(d, EventArgs.Empty);
        }

        private void MarkupRenderControl_OnUnloaded(object sender, RoutedEventArgs e)
        {
            _isUnloaded = true;
            Loaded -= MarkupRenderControl_OnLoaded;
            Unloaded -= MarkupRenderControl_OnUnloaded;
            SizeChanged -= MarkupRenderControl_OnSizeChanged;
            RenderHost?.RemoveFromVisualTree();
            RenderHost = null;
            RenderData = null;
            if (_isResumedSet)
            {
                if (Windows.Foundation.Metadata.ApiInformation.IsEventPresent("Windows.UI.Xaml.Application", nameof(Application.LeavingBackground)))
                {
                    Application.Current.LeavingBackground -= CurrentOnLeavingBackground;
                }
                else
                {
                    Application.Current.Resuming -= CurrentOnResuming;
                }
            }
        }

        private void MarkupRenderControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            InvalidateData(RenderData, GetActualWidth());
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
                    InvalidateData(RenderData, w);
                }
            });
        }

        private void ImageHost_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (_measureMap != null)
            {
                var pos = e.GetPosition(RenderHost);
                var command = _measureMap.TextAt(pos);
                if (command != null)
                {
                    TextTapped?.Invoke(this, command);
                }
            }
        }

        /// <summary>
        /// Find text at given position.
        /// </summary>
        /// <param name="pos">Position relative to control.</param>
        /// <returns>Found text.</returns>
        public IRenderCommand TextAt(Point pos)
        {
            return _measureMap?.TextAt(pos);
        }

        /// <summary>
        /// Text area tapped.
        /// </summary>
        public event EventHandler<IRenderCommand> TextTapped;

        /// <summary>
        /// ExceedsLines property changed.
        /// </summary>
        public event EventHandler ExceedsLinesChanged;

        private void RenderHost_OnRegionsInvalidated(CanvasVirtualControl sender, CanvasRegionsInvalidatedEventArgs args)
        {
            foreach (var r in args.InvalidatedRegions)
            {
                using (var session = sender.CreateDrawingSession(r))
                {
                    RenderMap(session, r);
                }
            }
        }
    }
}
