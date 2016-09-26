using System;
using System.Collections.Generic;
using Windows.System.Threading;

namespace Ipatov.WeakEvents
{
    /// <summary>
    /// Weak event channel.
    /// </summary>
    public sealed class WeakEventChannel : IWeakEventChannel
    {
        private readonly object _lock = new object();

        private const int MaxCount = 100;

        private readonly List<WeakEventContainer> _containers = new List<WeakEventContainer>();

        private static readonly List<WeakReference<WeakEventChannel>> Channels;

        private static readonly ThreadPoolTimer CleanupTask;

        static WeakEventChannel()
        {
            Channels = new List<WeakReference<WeakEventChannel>>();
            CleanupTask = ThreadPoolTimer.CreatePeriodicTimer(CleanupHandler, TimeSpan.FromMinutes(2));
        }

        private static void CleanupHandler(ThreadPoolTimer timer)
        {
            try
            {
                var channels = new List<WeakEventChannel>();
                var toDelete = new List<WeakReference<WeakEventChannel>>();
                lock (Channels)
                {
                    foreach (var channel in Channels)
                    {
                        WeakEventChannel c;
                        if (channel.TryGetTarget(out c))
                        {
                            channels.Add(c);
                        }
                        else
                        {
                            toDelete.Add(channel);
                        }
                    }
                    foreach (var channel in toDelete)
                    {
                        Channels.Remove(channel);
                    }
                }
                foreach (var channel in channels)
                {
                    channel.Cleanup();
                }
            }
            catch
            {
                // ignore
            }
        }

        /// <summary>
        /// Channel identifier.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Channel identifier.</param>
        public WeakEventChannel(Guid id)
        {
            Id = id;
            lock (Channels)
            {
                Channels.Add(new WeakReference<WeakEventChannel>(this));
            }
        }

        private void Cleanup()
        {
            lock (_lock)
            {
                var toDelete = new List<WeakEventContainer>();
                for (int i = 0; i < _containers.Count; i++)
                {
                    var container = _containers[i];
                    container.TryCleanup();
                    if (container.Count == 0 && i > 0)
                    {
                        toDelete.Add(container);
                    }
                }
                foreach (var container in toDelete)
                {
                    _containers.Remove(container);
                }
            }
        }

        /// <summary>
        /// Register callback.
        /// </summary>
        /// <param name="callback">Weak event callback.</param>
        /// <returns>Callback registration token.</returns>
        public Guid AddCallback(IWeakEventCallback callback)
        {
            lock (_lock)
            {
                if (_containers.Count == 0)
                {
                    _containers.Insert(0, new WeakEventContainer());
                }
                if (_containers[0].Count > MaxCount)
                {
                    _containers.Insert(0, new WeakEventContainer());
                }
                return _containers[0].Register(callback);
            }
        }

        /// <summary>
        /// Remove callback.
        /// </summary>
        /// <param name="token">Weak event callback registration token.</param>
        public void RemoveCallback(Guid token)
        {
            lock (_lock)
            {
                var toDelete = new List<WeakEventContainer>();
                for (int i = 0; i < _containers.Count; i++)
                {
                    var container = _containers[i];
                    container.Unregister(token);
                    if (container.Count == 0 && i > 0)
                    {
                        toDelete.Add(container);
                    }
                }
                foreach (var container in toDelete)
                {
                    _containers.Remove(container);
                }
            }
        }

        /// <summary>
        /// Trigger weak event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event argument.</param>
        public void RaiseEvent(object sender, object e)
        {
            var toInvoke = new List<IWeakEventCallback>();
            lock (_lock)
            {
                foreach (var container in _containers)
                {
                    foreach (var cb in container.GetCallbacks())
                    {
                        toInvoke.Add(cb);
                    }
                }
            }
            foreach (var cb in toInvoke)
            {
                cb.ReceiveWeakEvent(sender, this, e);
            }
        }

        private class WeakEventContainer
        {
            private Dictionary<Guid, WeakReference<IWeakEventCallback>> _references = new Dictionary<Guid, WeakReference<IWeakEventCallback>>();

            public int Count => _references.Count;

            public void TryCleanup()
            {
                var toDelete = new List<Guid>();
                foreach (var kv in _references)
                {
                    IWeakEventCallback cb;
                    if (!kv.Value.TryGetTarget(out cb))
                    {
                        toDelete.Add(kv.Key);
                    }
                }
                foreach (var id in toDelete)
                {
                    _references.Remove(id);
                }
            }

            public Guid Register(IWeakEventCallback callback)
            {
                if (callback == null)
                {
                    return Guid.Empty;
                }
                var id = Guid.NewGuid();
                _references[id] = new WeakReference<IWeakEventCallback>(callback);
                return id;
            }

            public void Unregister(Guid id)
            {
                _references.Remove(id);
            }

            public ICollection<IWeakEventCallback> GetCallbacks()
            {
                var toInvoke = new List<IWeakEventCallback>();
                var toDelete = new List<Guid>();
                foreach (var kv in _references)
                {
                    IWeakEventCallback cb;
                    if (!kv.Value.TryGetTarget(out cb))
                    {
                        toDelete.Add(kv.Key);
                    }
                    else
                    {
                        toInvoke.Add(cb);
                    }
                }
                foreach (var id in toDelete)
                {
                    _references.Remove(id);
                }
                return toInvoke;
            }
        }
    }
}