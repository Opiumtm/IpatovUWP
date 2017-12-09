using System.Collections.Generic;
using System.Threading;

namespace Ipatov.DataStructures
{
    public class BinarySortedListDictionary<TKey, TValue, TComparer> : SortedListDictionaryBase<TKey, TValue, TComparer>
        where TComparer : IComparer<TKey>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="comparer">Comparer.</param>
        public BinarySortedListDictionary(TComparer comparer) : base(comparer)
        {
        }

        /// <summary>
        /// Find key index.
        /// </summary>
        /// <param name="list">Source list.</param>
        /// <param name="key">Key.</param>
        /// <param name="foundIndex">Index of key of index or the first index lesser than a key. If (-1) then just add to the end of list.</param>
        /// <returns>true if found exact key match.</returns>
        protected override bool DoFindKeyIndex(List<KeyValuePair<TKey, TValue>> list, TKey key, out int foundIndex)
        {
            var lidx = 0;
            var ridx = list.Count - 1;
            do
            {
                if (lidx > ridx)
                {
                    foundIndex = lidx;
                    return false;
                }
                int midx = (lidx + ridx) / 2;
                var mkey = list[midx].Key;
                var compare = Comparer.Compare(mkey, key);
                if (compare < 0)
                {
                    lidx = midx + 1;
                } else if (compare > 0)
                {
                    ridx = midx - 1;
                }
                else
                {
                    foundIndex = midx;
                    return true;
                }
            } while (true);
        }
    }
}