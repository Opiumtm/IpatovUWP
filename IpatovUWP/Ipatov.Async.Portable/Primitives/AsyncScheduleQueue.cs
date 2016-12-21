using System.Collections.Generic;
using System.Linq;

namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async schedule list.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    public sealed class AsyncScheduleQueue<T>
    {
        private readonly Dictionary<AsyncSchedulePriority, List<T>> _data = new Dictionary<AsyncSchedulePriority, List<T>>();

        /// <summary>
        /// Enqueue element.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <param name="priority">Priority.</param>
        public void Enqueue(T data, AsyncSchedulePriority priority)
        {
            if (!_data.ContainsKey(priority))
            {
                _data[priority] = new List<T>();
            }
            _data[priority].Add(data);
        }

        /// <summary>
        /// Try dequeue data.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <returns>Element.</returns>
        public bool TryDequeue(out T data)
        {
            foreach (var key in _data.Keys.OrderByDescending(k => (int)k))
            {
                var data2 = _data[key];
                if (data2.Count > 0)
                {
                    data = data2[0];
                    data2.RemoveAt(0);
                    return true;
                }
            }
            data = default(T);
            return false;
        }

        /// <summary>
        /// Remove data at given priority.
        /// </summary>
        /// <param name="data">Data to remove.</param>
        /// <param name="priority">Priority.</param>
        /// <returns>true if successfully removed</returns>
        public bool Remove(T data, AsyncSchedulePriority? priority)
        {
            if (priority != null)
            {
                if (_data.ContainsKey(priority.Value))
                {
                    return _data[priority.Value].Remove(data);
                }
                return false;
            }
            foreach (var key in _data.Keys)
            {
                var data2 = _data[key];
                if (data2.Remove(data))
                {
                    return true;
                }
            }
            return false;
        }
    }
}