using System;

namespace Ipatov.DataStructures
{
    /// <summary>
    /// Optionally present key element.
    /// </summary>
    /// <typeparam name="TKeyElement">Type of key element</typeparam>
    public struct MaybeKeyElement<TKeyElement>
    {
        /// <summary>
        /// Key present.
        /// </summary>
        public readonly bool IsPresent;

        /// <summary>
        /// Element.
        /// </summary>
        public readonly TKeyElement Element;

        public MaybeKeyElement(bool isPresent, TKeyElement element)
        {
            IsPresent = isPresent;
            Element = element;
        }

        public static implicit operator MaybeKeyElement<TKeyElement>(TKeyElement element)
        {
            return new MaybeKeyElement<TKeyElement>(true, element);
        }

        public static MaybeKeyElement<TKeyElement> Empty()
        {
            return new MaybeKeyElement<TKeyElement>(false, default(TKeyElement));
        }
    }
}