using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Ipatov.BinarySerialization.Internals
{
    /// <summary>
    /// High-performance known strings index.
    /// </summary>
    internal sealed class KnownStringIndex
    {
        private readonly List<string> _strings = new List<string>();
        private readonly Dictionary<string, int> _indexes = new Dictionary<string, int>();

        /// <summary>
        /// Add reference.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <returns>Object index.</returns>
        public int AddString(string obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            var idx = FindIndex(obj);
            if (idx != null)
            {
                return idx.Value;
            }
            var cnt = _strings.Count;
            AddString(cnt, obj);
            return cnt;
        }

        /// <summary>
        /// Add reference.
        /// </summary>
        /// <param name="idx">Index.</param>
        /// <param name="obj">Object.</param>
        /// <returns>Object index.</returns>
        public void AddString(int idx, string obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            while (idx >= _strings.Count)
            {
                _strings.Add(null);
            }
            _strings[idx] = obj;
            SetIndex(obj, idx);
        }

        /// <summary>
        /// Test if reference already exists.
        /// </summary>
        /// <param name="obj">Object.</param>
        /// <returns>Index or null if not exists.</returns>
        public int? IsStringReferenced(string obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return FindIndex(obj);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetIndex(string obj, int idx)
        {
            _indexes[obj] = idx;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int? FindIndex(string obj)
        {
            if (_indexes.TryGetValue(obj, out var v))
            {
                return v;
            }
            return null;
        }

        /// <summary>
        /// Get object reference.
        /// </summary>
        /// <param name="index">Object index.</param>
        /// <returns>Object</returns>
        public string GetString(int index)
        {
            return (index >= 0 && index < _strings.Count) ? _strings[index] : null;
        }
    }
}