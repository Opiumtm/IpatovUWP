using System;
using System.Collections.Generic;

namespace Ipatov.DataStructures
{
    /// <summary>
    /// Key context for string.
    /// </summary>
    public sealed class StringTreeKeyContext : IPrefixTreeKeyContext<string, char, StringTreeKeyContext.KeyEnumerator>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="charComparer">Characters comparer.</param>
        public StringTreeKeyContext(IComparer<char> charComparer = null)
        {
            KeyElementComparer = charComparer ?? Comparer<char>.Default;
        }

        public IComparer<char> KeyElementComparer { get; }

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
    }
}