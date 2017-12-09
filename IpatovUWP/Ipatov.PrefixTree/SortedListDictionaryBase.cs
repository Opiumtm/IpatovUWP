using System;
using System.Collections;
using System.Collections.Generic;

namespace Ipatov.DataStructures
{
    /// <summary>
    /// Sorted list dictionary.
    /// </summary>
    /// <typeparam name="TKey">Key.</typeparam>
    /// <typeparam name="TValue">Value.</typeparam>
    /// <typeparam name="TComparer">Comparer.</typeparam>
    public abstract class SortedListDictionaryBase<TKey, TValue, TComparer> : IDictionary<TKey, TValue>
        where TComparer : IComparer<TKey>
    {
        private readonly List<KeyValuePair<TKey, TValue>> _internalList;

        private readonly TComparer _comparer;

        private readonly KeyValuePairComparer _keyValueComparer;

        protected SortedListDictionaryBase(TComparer comparer)
        {
            _comparer = comparer;
            _internalList = new List<KeyValuePair<TKey, TValue>>();
            _keyValueComparer = new KeyValuePairComparer(comparer);
        }

        protected TComparer Comparer => _comparer;

        protected KeyValuePairComparer KeyValueComparer => _keyValueComparer;

        /// <summary>
        /// Is list unsorted.
        /// </summary>
        protected bool IsUnsorted => false;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return EnumAll(kv => kv, true).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            AddOrUpdate(item.Key, item.Value, true);
        }

        public void Clear()
        {
            _internalList.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (FindKeyIndex(item.Key, out var idx))
            {
                var kv = _internalList[idx];
                return EqualityComparer<TValue>.Default.Equals(item.Value, kv.Value);
            }
            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (IsUnsorted)
            {
                var a = new KeyValuePair<TKey, TValue>[_internalList.Count];
                SortArray(a);
                a.CopyTo(array, arrayIndex);
            }
            _internalList.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (FindKeyIndex(item.Key, out var idx))
            {
                if (EqualityComparer<TValue>.Default.Equals(_internalList[idx].Value, item.Value))
                {
                    _internalList.RemoveAt(idx);
                    return true;
                }
            }
            return false;
        }

        public int Count => _internalList.Count;

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            AddOrUpdate(key, value, true);
        }

        public bool ContainsKey(TKey key)
        {
            return FindKeyIndex(key, out var _);
        }

        public bool Remove(TKey key)
        {
            if (FindKeyIndex(key, out var idx))
            {
                _internalList.RemoveAt(idx);
                return true;
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (FindKeyIndex(key, out var idx))
            {
                value = _internalList[idx].Value;
                return true;
            }
            value = default(TValue);
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out var v))
                {
                    return v;
                }
                throw new KeyNotFoundException();
            }
            set => AddOrUpdate(key, value, false);
        }

        public ICollection<TKey> Keys => EnumAllToList(kv => kv.Key, true);

        public ICollection<TValue> Values => EnumAllToList(kv => kv.Value, true);

        private IEnumerable<T> EnumAll<T>(Func<KeyValuePair<TKey, TValue>, T> selectFunc, bool forceSort)
        {
            if (IsUnsorted && forceSort)
            {
                var a = new KeyValuePair<TKey, TValue>[_internalList.Count];
                _internalList.CopyTo(a);
                SortArray(a);
                for (var i = 0; i < a.Length; i++)
                {
                    var kv = a[i];
                    yield return selectFunc(kv);
                }
                yield break;
            }
            for (var i = 0; i < _internalList.Count; i++)
            {
                var kv = _internalList[i];
                yield return selectFunc(kv);
            }
        }

        private List<T> EnumAllToList<T>(Func<KeyValuePair<TKey, TValue>, T> selectFunc, bool forceSort)
        {
            var r = new List<T>();
            if (IsUnsorted && forceSort)
            {
                var a = new KeyValuePair<TKey, TValue>[_internalList.Count];
                _internalList.CopyTo(a);
                SortArray(a);
                for (var i = 0; i < a.Length; i++)
                {
                    var kv = a[i];
                    r.Add(selectFunc(kv));
                }
                return r;
            }
            for (var i = 0; i < _internalList.Count; i++)
            {
                var kv = _internalList[i];
                r.Add(selectFunc(kv));
            }
            return r;
        }

        private void AddOrUpdate(TKey key, TValue value, bool throwIfExists)
        {
            var oldSortStatus = IsUnsorted;
            DoAddOrUpdate(key, value, throwIfExists);
            if (oldSortStatus && !IsUnsorted)
            {
                SortList(_internalList);
            }
        }

        private void DoAddOrUpdate(TKey key, TValue value, bool throwIfExists)
        {
            int idx;
            var exists = FindKeyIndex(key, out idx);
            if (exists)
            {
                if (throwIfExists)
                {
                    throw new InvalidOperationException("Key already exists in dictionary");
                }
                _internalList[idx] = new KeyValuePair<TKey, TValue>(key, value);
            }
            else
            {
                if (idx >= 0)
                {
                    _internalList.Insert(idx, new KeyValuePair<TKey, TValue>(key, value));
                }
                else
                {
                    _internalList.Add(new KeyValuePair<TKey, TValue>(key, value));
                }
            }
        }


        /// <summary>
        /// Sort array.
        /// </summary>
        /// <param name="toSort">Array to sort.</param>
        protected virtual void SortArray(KeyValuePair<TKey, TValue>[] toSort)
        {
            Array.Sort(toSort, _keyValueComparer);
        }

        /// <summary>
        /// Sort array.
        /// </summary>
        /// <param name="toSort">Array to sort.</param>
        protected virtual void SortList(List<KeyValuePair<TKey, TValue>> toSort)
        {
            toSort.Sort(_keyValueComparer);
        }

        private bool FindKeyIndex(TKey key, out int foundIndex)
        {
            if (!IsUnsorted)
            {
                return DoFindKeyIndex(_internalList, key, out foundIndex);
            }
            for (var i = 0; i < _internalList.Count; i++)
            {
                if (_comparer.Compare(key, _internalList[i].Key) == 0)
                {
                    foundIndex = i;
                    return true;
                }
            }
            foundIndex = -1;
            return false;
        }

        /// <summary>
        /// Find key index.
        /// </summary>
        /// <param name="list">Source list.</param>
        /// <param name="key">Key.</param>
        /// <param name="foundIndex">Index of key of index or the first index lesser than a key. If (-1) then just add to the end of list.</param>
        /// <returns>true if found exact key match.</returns>
        protected abstract bool DoFindKeyIndex(List<KeyValuePair<TKey, TValue>> list, TKey key, out int foundIndex);

        /// <summary>
        /// Proxy comparer.
        /// </summary>
        protected class KeyValuePairComparer : IComparer<KeyValuePair<TKey, TValue>>
        {
            private readonly TComparer _comparer;

            public KeyValuePairComparer(TComparer comparer)
            {
                _comparer = comparer;
            }

            /// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
            /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.</returns>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
            {
                return _comparer.Compare(x.Key, y.Key);
            }
        }
    }
}