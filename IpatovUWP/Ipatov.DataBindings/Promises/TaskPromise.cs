using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Ipatov.DataBindings
{
    /// <summary>
    /// Promise wrapper for task.
    /// </summary>
    /// <typeparam name="T">Type of data value.</typeparam>
    public sealed class TaskPromise<T> : DataPromiseBase<T>
    {
        private readonly Task<T> _task;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="task">Task.</param>
        public TaskPromise(Task<T> task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            _task = task;
        }

        private readonly ConditionalWeakTable<IDataPromiseCallback<T>, Task> _callbackTasks = new ConditionalWeakTable<IDataPromiseCallback<T>, Task>();

        private readonly object _lock = new object();

        /// <summary>
        /// Continue data promise.
        /// </summary>
        /// <param name="callback">Data promise callback.</param>
        public override void Continue(IDataPromiseCallback<T> callback)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            Task existing;
            if (!_callbackTasks.TryGetValue(callback, out existing))
            {
                Guid token;
                lock (_lock)
                {
                    token = RegisterCallback(callback);
                }
                var task = _task.ContinueWith((t, o) =>
                {
                    try
                    {
                        lock (_lock)
                        {
                            ExtractCallback(token);
                        }
                        if (t.IsFaulted)
                        {
                            callback.PromiseError(t.Exception);
                        }
                        else if (t.IsCanceled)
                        {
                            callback.PromiseError(new TaskCanceledException());
                        }
                        else if (t.IsCompleted)
                        {
                            callback.ValueAvailable(t.Result);
                        }
                        else
                        {
                            callback.PromiseError(new InvalidOperationException("Invalid promise task state"));
                        }
                    }
                    catch (Exception ex)
                    {
                        callback.PromiseError(ex);
                    }
                }, null);
                _callbackTasks.Add(callback, task);
            }
        }
    }
}