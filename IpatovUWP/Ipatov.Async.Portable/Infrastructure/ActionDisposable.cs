using System;
using System.Threading;

namespace Ipatov.Async
{
    /// <summary>
    /// Action disposable.
    /// </summary>
    internal class ActionDisposable : IDisposable
    {
        private readonly Action _onDispose;

        private int _isDisposed = 0;

        private readonly bool _disposeOnce;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="onDispose">Action on dispose.</param>
        /// <param name="disposeOnce">Dispose only once.</param>
        public ActionDisposable(Action onDispose, bool disposeOnce = true)
        {
            _onDispose = onDispose;
            _disposeOnce = disposeOnce;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (!_disposeOnce)
            {
                _onDispose?.Invoke();
            }
            else
            {
                if (Interlocked.Exchange(ref _isDisposed, 1) == 0)
                {
                    _onDispose?.Invoke();
                }
            }
        }
    }
}