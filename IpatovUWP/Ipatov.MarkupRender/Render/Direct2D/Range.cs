using System.Collections.Generic;

namespace Ipatov.MarkupRender.Direct2D
{
    /// <summary>
    /// Range.
    /// </summary>
    internal struct Range
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="begin">Range start.</param>
        /// <param name="count">Elements count.</param>
        public Range(int begin, int count)
        {
            Begin = begin;
            Count = count;
        }

        /// <summary>
        /// Range start.
        /// </summary>
        public int Begin;

        /// <summary>
        /// Elements count.
        /// </summary>
        public int Count;

        /// <summary>
        /// Range end.
        /// </summary>
        public int End => Begin + Count - 1;

        /// <summary>
        /// Check if ranges is overlapping.
        /// </summary>
        /// <param name="other">Other range.</param>
        /// <returns>true if overlapping.</returns>
        public bool InterlappedWith(Range other)
        {
            return !(End < other.Begin || Begin > other.End);
        }

        /// <summary>
        /// Convert change indices to ranges.
        /// </summary>
        /// <param name="c">Change indices.</param>
        /// <param name="totalLength">Total length.</param>
        /// <returns>Character ranges.</returns>
        public static IEnumerable<Range> ChangeIndicesToRanges(int[] c, int totalLength)
        {
            if (c.Length == 0)
            {
                yield return new Range(0, totalLength);
                yield break;
            }
            for (int i = 0; i < c.Length - 1; i++)
            {
                yield return new Range(c[i], c[i + 1] - c[i]);
            }
        }
    }
}