using System;
using System.Collections.Generic;

namespace Ipatov.DataStructures
{
    public sealed class ArrayTreeKeyContext<TKeyElement> : IPrefixTreeKeyContext<TKeyElement[], TKeyElement, ArrayTreeKeyContext<TKeyElement>.KeyEnumerator>
    {
        /// <summary>
        /// Key enumerator.
        /// </summary>
        public struct KeyEnumerator : IKeyElementsEnumerator<TKeyElement>
        {
            private readonly TKeyElement[] _str;
            private int _cnt;
            private readonly int _len;

            public KeyEnumerator(TKeyElement[] str) : this()
            {
                _str = str ?? throw new ArgumentNullException(nameof(str));
                _cnt = 0;
                _len = str.Length;
            }

            public MaybeKeyElement<TKeyElement> GetNextKeyElement()
            {
                if (_cnt < _len)
                {
                    var r = _str[_cnt];
                    _cnt++;
                    return r;
                }
                return MaybeKeyElement<TKeyElement>.Empty();
            }
        }

        public ArrayTreeKeyContext(IComparer<TKeyElement> keyElementComparer)
        {
            KeyElementComparer = keyElementComparer ?? Comparer<TKeyElement>.Default;
        }

        /// <summary>
        /// Key element comparer.
        /// </summary>
        public IComparer<TKeyElement> KeyElementComparer { get; }

        public TKeyElement[] ComposeKey(List<TKeyElement> keyElements)
        {
            var a = new TKeyElement[keyElements.Count];
            keyElements.CopyTo(a);
            return a;
        }

        public KeyEnumerator Enumerate(TKeyElement[] key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            return new KeyEnumerator(key);
        }
    }
}