using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ipatov.BinarySerialization.Internals
{
    /// <summary>
    /// Compound dictionary.
    /// </summary>
    /// <typeparam name="TKey">Type of key.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    internal sealed class CompoundDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        private readonly IReadOnlyDictionary<TKey, TValue>[] _sourceDictionaries;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sourceDictionaries">Source dictionaries.</param>
        public CompoundDictionary(params IReadOnlyDictionary<TKey, TValue>[] sourceDictionaries)
        {
            if (sourceDictionaries == null) throw new ArgumentNullException(nameof(sourceDictionaries));
            _sourceDictionaries = sourceDictionaries.Where(d => d != null).ToArray();
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return GetKeyValuePairs().GetEnumerator();
        }

        private IEnumerable<KeyValuePair<TKey, TValue>> GetKeyValuePairs()
        {
            foreach (var key in Keys)
            {
                foreach (var d in _sourceDictionaries.Reverse())
                {
                    if (d.ContainsKey(key))
                    {
                        yield return new KeyValuePair<TKey, TValue>(key, d[key]);
                        break;
                    }
                }
            }
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>Gets the number of elements in the collection.</summary>
        /// <returns>The number of elements in the collection. </returns>
        public int Count => Keys.Count();

        /// <summary>Determines whether the read-only dictionary contains an element that has the specified key.</summary>
        /// <returns>true if the read-only dictionary contains an element that has the specified key; otherwise, false.</returns>
        /// <param name="key">The key to locate.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="key" /> is null.</exception>
        public bool ContainsKey(TKey key)
        {
            return _sourceDictionaries.Any(d => d.ContainsKey(key));
        }

        /// <summary>Gets the value that is associated with the specified key.</summary>
        /// <returns>true if the object that implements the <see cref="T:System.Collections.Generic.IReadOnlyDictionary`2" /> interface contains an element that has the specified key; otherwise, false.</returns>
        /// <param name="key">The key to locate.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="key" /> is null.</exception>
        public bool TryGetValue(TKey key, out TValue value)
        {
            foreach (var d in _sourceDictionaries.Reverse())
            {
                if (d.TryGetValue(key, out value))
                {
                    return true;
                }
            }
            value = default(TValue);
            return false;
        }

        /// <summary>Gets the element that has the specified key in the read-only dictionary.</summary>
        /// <returns>The element that has the specified key in the read-only dictionary.</returns>
        /// <param name="key">The key to locate.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="key" /> is null.</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key" /> is not found. </exception>
        public TValue this[TKey key]
        {
            get
            {
                foreach (var d in _sourceDictionaries.Reverse())
                {
                    if (d.ContainsKey(key))
                    {
                        return d[key];
                    }
                }
                throw new KeyNotFoundException();
            }
        }

        /// <summary>Gets an enumerable collection that contains the keys in the read-only dictionary. </summary>
        /// <returns>An enumerable collection that contains the keys in the read-only dictionary.</returns>
        public IEnumerable<TKey> Keys => _sourceDictionaries.SelectMany(d => d.Keys).Distinct();

        /// <summary>Gets an enumerable collection that contains the values in the read-only dictionary.</summary>
        /// <returns>An enumerable collection that contains the values in the read-only dictionary.</returns>
        public IEnumerable<TValue> Values
        {
            get
            {
                foreach (var key in Keys)
                {
                    foreach (var d in _sourceDictionaries.Reverse())
                    {
                        if (d.ContainsKey(key))
                        {
                            yield return d[key];
                            break;
                        }
                    }
                }
            }
        }
    }
}