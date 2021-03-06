﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.Async.Messaging
{
    /// <summary>
    /// Blocking message channel. Sender would wait until message is received and replied.
    /// </summary>
    public sealed class BlockingChannel<TMsg, TReply> : IAsyncChannel<TMsg, TReply>
    {
        private int _isDisposed = 0;

        private int _disposedMode = (int) BlockingChannelDisposedMode.Discard;

        /// <summary>
        /// Disposed mode.
        /// </summary>
        public BlockingChannelDisposedMode DisposedMode
        {
            get { return (BlockingChannelDisposedMode) Interlocked.CompareExchange(ref _disposedMode, 0, 0); }
            set { Interlocked.Exchange(ref _disposedMode, (int) value); }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Interlocked.Exchange(ref _isDisposed, 1);
        }

        private bool IsDisposed()
        {
            return Interlocked.CompareExchange(ref _isDisposed, 0, 0) != 0;
        }

        /// <inheritdoc />
        public Task<TReply> Send(TMsg msg, int priority = 0)
        {
            return Send(msg, new CancellationToken(), priority);
        }

        /// <inheritdoc />
        public Task<IAsyncMessage<TMsg, TReply>> Receive(int priority = 0)
        {
            return Receive(new CancellationToken(), priority);
        }

        /// <inheritdoc />
        public Task<TReply> Send(TMsg msg, CancellationToken token, int priority = 0)
        {
            if (token.IsCancellationRequested)
            {
                return Task.FromCanceled<TReply>(token);
            }
            if (IsDisposed())
            {
                switch (DisposedMode)
                {
                    case BlockingChannelDisposedMode.ImmediateException:
                        throw new ObjectDisposedException(nameof(BlockingChannel<TMsg, TReply>));
                    case BlockingChannelDisposedMode.FaultedTask:
                        return Task.FromException<TReply>(new ObjectDisposedException(nameof(BlockingChannel<TMsg, TReply>)));
                    default:
                        return Task.FromResult(default(TReply));
                }                
            }
            var toExecute = new List<Action>();
            var message = new AsyncMessage<TMsg, TReply>(msg, token, priority, StateChangedCallback);
            lock (_stateLock)
            {
                var waiter = _waiters.OrderByDescending(w => w.Priority).FirstOrDefault();
                if (waiter != null)
                {
                    _waiters.Remove(waiter);
                    toExecute.Add(() =>
                    {
                        waiter.Waiter.TrySetResult(message);
                    });
                }
                else
                {
                    _incoming.Add(message);
                }
            }
            foreach (var action in toExecute)
            {
                action();
            }
            return message.GetTask();
        }

        private void StateChangedCallback(AsyncMessage<TMsg, TReply> asyncMessage, AsyncMessageStateChange asyncMessageStateChange)
        {
            lock (_stateLock)
            {
                _incoming.Remove(asyncMessage);
            }
        }

        /// <inheritdoc />
        public Task<IAsyncMessage<TMsg, TReply>> Receive(CancellationToken token, int priority = 0)
        {
            if (token.IsCancellationRequested)
            {
                return Task.FromCanceled<IAsyncMessage<TMsg, TReply>>(token);
            }
            if (IsDisposed())
            {
                switch (DisposedMode)
                {
                    case BlockingChannelDisposedMode.ImmediateException:
                        throw new ObjectDisposedException(nameof(BlockingChannel<TMsg, TReply>));
                    case BlockingChannelDisposedMode.FaultedTask:
                        return Task.FromException<IAsyncMessage<TMsg, TReply>>(new ObjectDisposedException(nameof(BlockingChannel<TMsg, TReply>)));
                    default:
                        return Task.FromResult(default(IAsyncMessage<TMsg, TReply>));
                }
            }
            var toExecute = new List<Action>();
            Task<IAsyncMessage<TMsg, TReply>> result;
            lock (_stateLock)
            {
                var message = _incoming.OrderByDescending(p => p.Priority).FirstOrDefault();
                if (message != null)
                {
                    _incoming.Remove(message);
                    IAsyncMessage<TMsg, TReply> msg = message;
                    result = Task.FromResult(msg);
                }
                else
                {
                    var tcs = new TaskCompletionSource<IAsyncMessage<TMsg, TReply>>();
                    var waiter = new WaiterInfo()
                    {
                        Priority = priority,
                        Waiter = tcs
                    };
                    _waiters.Add(waiter);
                    toExecute.Add(() =>
                    {
                        token.Register(() =>
                        {
                            tcs.TrySetCanceled(token);
                            lock (_stateLock)
                            {
                                _waiters.Remove(waiter);
                            }
                        });
                    });
                    result = tcs.Task;
                }
            }
            foreach (var action in toExecute)
            {
                action();
            }
            return result;
        }

        private readonly object _stateLock = new object();

        private readonly List<AsyncMessage<TMsg, TReply>> _incoming = new List<AsyncMessage<TMsg, TReply>>();

        private readonly List<WaiterInfo> _waiters = new List<WaiterInfo>();

        private class WaiterInfo
        {
            public int Priority;
            public TaskCompletionSource<IAsyncMessage<TMsg, TReply>> Waiter;
        }
    }
}