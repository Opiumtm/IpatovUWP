using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Ipatov.Collections
{
    /// <summary>
    /// Dictionary with internal tree.
    /// </summary>
    /// <typeparam name="TKey">Type of key.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    /// <typeparam name="TTreeKey">Type of tree key.</typeparam>
    /// <typeparam name="TTreeProvider">Type of tree key provider.</typeparam>
    public sealed class TreeDictionary<TKey, TValue, TTreeKey, TTreeProvider> : IDictionary<TKey, TValue>
        where TTreeProvider : ITreeProvider<TKey, TTreeKey>
        where TTreeKey : struct, IEquatable<TTreeKey>
    {
        private readonly TTreeProvider _treeProvider;

        private readonly TreeDictionaryNode<TTreeKey> _emptyNode = new TreeDictionaryNode<TTreeKey>() { ListIndex = null, Level = -1 };

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="treeProvider">Tree provider.</param>
        public TreeDictionary(TTreeProvider treeProvider)
        {
            if (treeProvider == null) throw new ArgumentNullException(nameof(treeProvider));
            _treeProvider = treeProvider;
        }

        private TreeDictionaryNode<TTreeKey> _root = new TreeDictionaryNode<TTreeKey>() { Level = -1, ListIndex = null };
        private readonly List<TreeDictionaryListNode<TKey, TValue>> _list = new List<TreeDictionaryListNode<TKey, TValue>>();
        private readonly Queue<int> _freeIndexes = new Queue<int>();
        private int _count;

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return GetEnumerable().GetEnumerator();
        }

        private IEnumerable<KeyValuePair<TKey, TValue>> GetEnumerable()
        {
            for (var i = 0; i < _list.Count; i++)
            {
                var v = _list[i];
                if (v.IsAssigned)
                {
                    yield return new KeyValuePair<TKey, TValue>(v.Key, v.Value);
                }
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _list.Clear();
            _root = new TreeDictionaryNode<TTreeKey>() {Level = -1, ListIndex = null};
            _freeIndexes.Clear();
            _count = 0;
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            int acnt = arrayIndex;
            for (var i = 0; i < _list.Count; i++)
            {
                var v = _list[i];
                if (v.IsAssigned)
                {
                    array[acnt] = new KeyValuePair<TKey, TValue>(v.Key, v.Value);
                    acnt++;
                }
            }
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        /// <inheritdoc />
        public int Count => _count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public void Add(TKey key, TValue value)
        {
            Add(key, value, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Add(TKey key, TValue value, bool throwOnExists)
        {
            var levels = _treeProvider.GetLevelCount(key);
            if (levels == 0)
            {
                return;
            }
            TreeDictionaryNode<TTreeKey> curHoldeer = _root;
            TreeDictionaryNode<TTreeKey> parentHoldeer = _root;
            ref TreeDictionaryNode<TTreeKey> current = ref curHoldeer;
            ref TreeDictionaryNode<TTreeKey> parent = ref parentHoldeer;
            int nextLevelIndex = -1;
            for (var i = 0; i < levels; i++)
            {
                if (current.Children == null)
                {
                    current.Children = new List<TreeDictionaryNode<TTreeKey>>();
                    if (i > 0)
                    {
                        parent.Children[nextLevelIndex] = current;
                    }
                }
                parent = current;
                var levelKey = _treeProvider.GetKey(key, i);
                nextLevelIndex = current.Children.FindIndex(tk => tk.Key.Equals(levelKey));
                if (nextLevelIndex < 0)
                {
                    var newValue = new TreeDictionaryNode<TTreeKey>()
                    {
                        Level = i,
                        ListIndex = null,
                        Children = null,
                        Key = levelKey
                    };
                    parent.Children.Add(newValue);
                    nextLevelIndex = parent.Children.Count - 1;
                }
                else
                {
                    current = parent.Children[nextLevelIndex];
                }
            }
            if (current.ListIndex != null)
            {
                if (throwOnExists)
                {
                    throw new ArgumentException("Key already present in dictionary");
                }
                var v = _list[current.ListIndex.Value];
                v.IsAssigned = true;
                v.Value = value;
                _list[current.ListIndex.Value] = v;
            }
            else
            {
                int fi;
                if (_freeIndexes.Count > 0)
                {
                    fi = _freeIndexes.Dequeue();
                    var v = _list[fi];
                    v.IsAssigned = true;
                    v.Value = value;
                    v.Key = key;
                    _list[fi] = v;
                }
                else
                {
                    _list.Add(new TreeDictionaryListNode<TKey, TValue>()
                    {
                        IsAssigned = true,
                        Key = key,
                        Value = value
                    });
                    fi = _list.Count - 1;
                }
                if (current.Level >= 0)
                {
                    var c = parent.Children[nextLevelIndex];
                    c.ListIndex = fi;
                    parent.Children[nextLevelIndex] = c;
                }
                else
                {
                    current.ListIndex = fi;
                }
                _count++;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Find(TKey key, ref TreeDictionaryNode<TTreeKey> value, ref TreeDictionaryNode<TTreeKey> parent)
        {
            var levels = _treeProvider.GetLevelCount(key);
            if (levels == 0)
            {
                return;
            }
            for (var i = 0; i < levels; i++)
            {
                if (value.Children == null)
                {
                    value =_emptyNode;
                    return;
                }
                var levelKey = _treeProvider.GetKey(key, i);
                var nextLevelIndex = value.Children.FindIndex(tk => tk.Key.Equals(levelKey));
                if (nextLevelIndex < 0)
                {
                    value = _emptyNode;
                    return;
                }
                parent = value;
                value = value.Children[nextLevelIndex];
            }
        }

        /// <inheritdoc />
        public bool ContainsKey(TKey key)
        {
            TreeDictionaryNode<TTreeKey> value = _root;
            TreeDictionaryNode<TTreeKey> parent = _root;
            Find(key, ref value, ref parent);
            return value.ListIndex != null;
        }

        /// <inheritdoc />
        public bool Remove(TKey key)
        {
            TreeDictionaryNode<TTreeKey> value = _root;
            TreeDictionaryNode<TTreeKey> parent = _root;
            Find(key, ref value, ref parent);
            if (value.ListIndex == null)
            {
                return false;
            }
            var v = _list[value.ListIndex.Value];
            v.IsAssigned = false;
            _list[value.ListIndex.Value] = v;
            _freeIndexes.Enqueue(value.ListIndex.Value);
            _count--;
            value.ListIndex = null;
            if (value.Level >= 0)
            {
                if (parent.Children != null)
                {
                    var levelKey = _treeProvider.GetKey(key, value.Level);
                    var index = value.Children.FindIndex(tk => tk.Key.Equals(levelKey));
                    if (index >= 0)
                    {
                        parent.Children.RemoveAt(index);
                    }
                }
            }
            return true;
        }

        /// <inheritdoc />
        public bool TryGetValue(TKey key, out TValue value)
        {
            TreeDictionaryNode<TTreeKey> value1 = _root;
            TreeDictionaryNode<TTreeKey> parent = _root;
            Find(key, ref value1, ref parent);
            if (value1.ListIndex == null)
            {
                value = default(TValue);
                return false;
            }
            var v = _list[value1.ListIndex.Value];
            if (!v.IsAssigned)
            {
                value = default(TValue);
                return false;
            }
            value = _list[value1.ListIndex.Value].Value;
            return true;
        }

        /// <inheritdoc />
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
            set => Add(key, value, false);
        }

        /// <inheritdoc />
        public ICollection<TKey> Keys
        {
            get
            {
                List<TKey> keys = new List<TKey>();
                for (var i = 0; i < _list.Count; i++)
                {
                    var v = _list[i];
                    if (v.IsAssigned)
                    {
                        keys.Add(v.Key);
                    }
                }
                return keys;
            }
        }

        /// <inheritdoc />
        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> values = new List<TValue>();
                for (var i = 0; i < _list.Count; i++)
                {
                    var v = _list[i];
                    if (v.IsAssigned)
                    {
                        values.Add(v.Value);
                    }
                }
                return values;
            }
        }
    }
}