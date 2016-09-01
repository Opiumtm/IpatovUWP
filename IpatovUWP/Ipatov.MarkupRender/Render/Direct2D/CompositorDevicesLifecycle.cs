using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Display;
using Windows.UI.Composition;
using Windows.UI.Core;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;

namespace Ipatov.MarkupRender.Direct2D
{
    /// <summary>
    /// Lifecycle of compositor devices.
    /// </summary>
    public sealed class CompositorDevicesLifecycle : IDisposable, ICompositionGraphicsDeviceSource
    {
        private readonly Compositor _compositor;

        private CanvasDevice _device;

        private CompositionGraphicsDevice _compositionGraphicsDevice;

        private readonly bool _handleSystemEvents;

        private bool _isDisposed;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="handleSystemEvents">Handle system-wide Direct2D events.</param>
        public CompositorDevicesLifecycle(bool handleSystemEvents = true)
        {
            _handleSystemEvents = handleSystemEvents;
            _compositor = new Compositor();
            if (handleSystemEvents)
            {
                CoreApplication.Suspending += CoreApplicationOnSuspending;
                DisplayInformation.DisplayContentsInvalidated += DisplayInformationOnDisplayContentsInvalidated;
            }
            CreateDevice();
        }

        /// <summary>
        /// Get drawing device.
        /// </summary>
        /// <returns>Drawing device.</returns>
        public CompositionGraphicsDevice GetDevice()
        {
            if (_isDisposed)
            {
                return null;
            }
            return _compositionGraphicsDevice;
        }

        /// <summary>
        /// Get compositor.
        /// </summary>
        /// <returns>Compositor.</returns>
        public Compositor GetCompositor()
        {
            if (_isDisposed)
            {
                return null;
            }
            return _compositor;
        }

        public event EventHandler Disposed;

        private void DisplayInformationOnDisplayContentsInvalidated(DisplayInformation sender, object args)
        {
            // The display contents could be invalidated due to a lost device, or for some other reason.
            // We check this by calling GetSharedDevice, which will make sure the device is still valid before returning it.
            // If the shared device has been lost, GetSharedDevice will automatically raise its DeviceLost event.
            CanvasDevice.GetSharedDevice();
        }

        private void CoreApplicationOnSuspending(object sender, SuspendingEventArgs suspendingEventArgs)
        {
            try
            {
                _device.Trim();
            }
            catch (Exception e) when (_device.IsDeviceLost(e.HResult))
            {
                _device.RaiseDeviceLost();
            }
        }

        private void CreateDevice()
        {
            if (_isDisposed)
            {
                return;
            }
            _device = CanvasDevice.GetSharedDevice();
            _device.DeviceLost += DeviceOnDeviceLost;
            if (_compositionGraphicsDevice == null)
            {
                _compositionGraphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(_compositor, _device);
            }
            else
            {
                CanvasComposition.SetCanvasDevice(_compositionGraphicsDevice, _device);
            }
        }

        private void DeviceOnDeviceLost(CanvasDevice sender, object args)
        {
            _device.DeviceLost -= DeviceOnDeviceLost;
            var unwaitedTask = CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, CreateDevice);
        }

        /// <summary>
        /// Dispose object.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;
            if (_handleSystemEvents)
            {
                CoreApplication.Suspending -= CoreApplicationOnSuspending;
                DisplayInformation.DisplayContentsInvalidated -= DisplayInformationOnDisplayContentsInvalidated;
            }
            _compositionGraphicsDevice.Dispose();
            _compositionGraphicsDevice = null;
            _compositor.Dispose();
            if (_device != null)
            {
                _device.DeviceLost -= DeviceOnDeviceLost;
                _device = null;
            }
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}