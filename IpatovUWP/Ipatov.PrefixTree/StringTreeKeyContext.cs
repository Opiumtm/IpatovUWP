using System;
using System.Collections.Generic;
using System.Globalization;

namespace Ipatov.DataStructures
{
    /// <summary>
    /// Key context for string.
    /// </summary>
    public sealed class StringTreeKeyContext : IPrefixTreeKeyContext<string, char, StringTreeKeyContext.KeyEnumerator, StringTreeKeyContext.KeyComparer>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ignoreCase">Ignore character case.</param>
        public StringTreeKeyContext(bool ignoreCase)
        {
            KeyElementComparer = new KeyComparer(ignoreCase);
        }

        public StringTreeKeyContext.KeyComparer KeyElementComparer { get; }

        /// <summary>
        /// Compose key from key elements.
        /// </summary>
        /// <param name="keyElements">Key elements.</param>
        /// <returns>Composed key.</returns>
        public string ComposeKey(List<char> keyElements)
        {
            if (keyElements == null) throw new ArgumentNullException(nameof(keyElements));
            var a = new char[keyElements.Count];
            keyElements.CopyTo(a);
            return new string(a);
        }

        /// <summary>
        /// Enumerate key elements.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Key elements enumerator.</returns>
        public KeyEnumerator Enumerate(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            return new KeyEnumerator(key);
        }

        /// <summary>
        /// String tree key enumerator.
        /// </summary>
        public struct KeyEnumerator : IKeyElementsEnumerator<char>
        {
            private readonly string _str;
            private int _cnt;
            private readonly int _len;

            public KeyEnumerator(string str) : this()
            {
                _str = str ?? throw new ArgumentNullException(nameof(str));
                _cnt = 0;
                _len = str.Length;
            }

            public MaybeKeyElement<char> GetNextKeyElement()
            {
                if (_cnt < _len)
                {
                    var r = _str[_cnt];
                    _cnt++;
                    return r;
                }
                return MaybeKeyElement<char>.Empty();
            }
        }

        public struct KeyComparer : IComparer<char>
        {
            private readonly bool _ignoreCase;

            public KeyComparer(bool ignoreCase)
            {
                _ignoreCase = ignoreCase;
            }

            public int Compare(char x, char y)
            {
                if (_ignoreCase)
                {
                    return char.ToLowerInvariant(x) - char.ToLowerInvariant(y);
                }
                return x - y;
            }
        }
    }
}