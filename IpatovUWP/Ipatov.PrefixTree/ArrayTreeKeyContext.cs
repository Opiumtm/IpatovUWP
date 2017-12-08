using System;
using System.Collections.Generic;

namespace Ipatov.DataStructures
{
    public sealed class ArrayTreeKeyContext<TKeyElement, TKeyComparer> : IPrefixTreeKeyContext<TKeyElement[], TKeyElement, ArrayTreeKeyContext<TKeyElement, TKeyComparer>.KeyEnumerator, TKeyComparer>
        where TKeyComparer : IComparer<TKeyElement>
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

        public ArrayTreeKeyContext(TKeyComparer keyElementComparer)
        {
            if (keyElementComparer == null)
            {
                throw new ArgumentNullException(nameof(keyElementComparer));
            }
            KeyElementComparer = keyElementComparer;
        }

        /// <summary>
        /// Key element comparer.
        /// </summary>
        public TKeyComparer KeyElementComparer { get; }

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