namespace Ipatov.Async.Primitives
{
    /// <summary>
    /// Async context schedule priority.
    /// </summary>
    public enum AsyncSchedulePriority
    {
        /// <summary>
        /// Ultra low priority.
        /// </summary>
        UltraLow = -2,

        /// <summary>
        /// Low priority.
        /// </summary>
        Low = -1,

        /// <summary>
        /// Default priority.
        /// </summary>
        Default = 0,

        /// <summary>
        /// High priority.
        /// </summary>
        High = 1,

        /// <summary>
        /// Ultra high priority.
        /// </summary>
        UltraHigh = 2
    }
}