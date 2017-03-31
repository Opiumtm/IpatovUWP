using System;
using System.Collections.Generic;

namespace Ipatov.BinarySerialization.Internals
{
    /// <summary>
    /// Lazy dictionary.
    /// </summary>
    /// <typeparam name="TKey">Key.</typeparam>
    /// <typeparam name="TValue">Value.</typeparam>
    internal sealed class LazyDictionary<TKey, TValue>
    {        
        private readonly Dictionary<TKey, TValue> _internalDictionary;

        private readonly Func<TKey, TValue> _getValueFunc;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="getValueFunc">Value function.</param>
        public LazyDictionary(Func<TKey, TValue> getValueFunc)
        {
            if (getValueFunc == null) throw new ArgumentNullException(nameof(getValueFunc));
            _internalDictionary = new Dictionary<TKey, TValue>();
            _getValueFunc = getValueFunc;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="getValueFunc">Value function.</param>
        /// <param name="keyComparer">Key comparer.</param>
        public LazyDictionary(Func<TKey, TValue> getValueFunc, IEqualityComparer<TKey> keyComparer)
        {
            if (getValueFunc == null) throw new ArgumentNullException(nameof(getValueFunc));
            _internalDictionary = new Dictionary<TKey, TValue>(keyComparer ?? EqualityComparer<TKey>.Default);
            _getValueFunc = getValueFunc;
        }

        /// <summary>
        /// Lazy value.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Value.</returns>
        public TValue this[TKey key]
        {
            get
            {
                lock (_internalDictionary)
                {
                    if (!_internalDictionary.ContainsKey(key))
                    {
                        _internalDictionary[key] = _getValueFunc(key);
                    }
                    return _internalDictionary[key];
                }
            }
        }
    }
}