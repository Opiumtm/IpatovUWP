using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding group.
    /// </summary>
    public sealed class DataBindingGroup : IDataBindingGroup
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        private readonly HashSet<IDataBinding> _bindings = new HashSet<IDataBinding>();

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            IDataBinding[] bindings;
            _lock.EnterWriteLock();
            try
            {
                bindings = _bindings.ToArray();
                _bindings.Clear();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
            foreach (var binding in bindings)
            {
                try
                {
                    binding.Error -= BindingOnError;
                    binding.Success -= BindingOnSuccess;
                    binding.Dispose();
                }
                catch (Exception ex)
                {
                    Error?.Invoke(this, ex);
                }
            }
        }

        public void Trigger()
        {
            IDataBinding[] bindings;
            _lock.EnterReadLock();
            try
            {
                bindings = _bindings.ToArray();
            }
            finally
            {
                _lock.ExitReadLock();
            }
            foreach (var binding in bindings)
            {
                try
                {
                    binding.Trigger();
                }
                catch (Exception ex)
                {
                    Error?.Invoke(this, ex);
                }
            }
        }

        /// <summary>
        /// Data binding error.
        /// </summary>
        public event EventHandler<Exception> Error;

        /// <summary>
        /// Data transfer success.
        /// </summary>
        public event EventHandler Success;

        public void AddBinding(IDataBinding binding)
        {
            if (binding != null)
            {
                bool isAdded = false;
                _lock.EnterWriteLock();
                try
                {
                    if (!_bindings.Contains(binding))
                    {
                        _bindings.Add(binding);
                        isAdded = true;
                    }
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
                if (isAdded)
                {
                    binding.Error += BindingOnError;
                    binding.Success += BindingOnSuccess;
                }
            }
        }

        private void BindingOnSuccess(object sender, EventArgs eventArgs)
        {
            Success?.Invoke(sender, eventArgs);
        }

        private void BindingOnError(object sender, Exception exception)
        {
            Error?.Invoke(sender, exception);
        }
    }
}