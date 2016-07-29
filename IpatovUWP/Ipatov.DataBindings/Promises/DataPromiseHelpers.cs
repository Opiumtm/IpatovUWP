using System;
using System.Threading.Tasks;

namespace Ipatov.DataBindings.Helpers
{
    /// <summary>
    /// Data promise helpers.
    /// </summary>
    public static class DataPromiseHelpers
    {
        /// <summary>
        /// Convert data promise to task.
        /// </summary>
        /// <typeparam name="T">Data value type.</typeparam>
        /// <param name="promise">Data promise.</param>
        /// <returns>Task.</returns>
        public static Task<T> AsTask<T>(this IDataPromise<T> promise)
        {
            if (promise == null) throw new ArgumentNullException(nameof(promise));
            var callback = new DataPromiseTaskCallback<T>();
            promise.Continue(callback);
            return callback.Task;
        }
    }
}