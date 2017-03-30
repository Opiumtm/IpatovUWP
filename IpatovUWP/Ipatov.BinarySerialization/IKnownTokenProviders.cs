namespace Ipatov.BinarySerialization
{
    /// <summary>
    /// Known token providers info.
    /// </summary>
    public interface IKnownTokenProviders
    {
        /// <summary>
        /// Get known token providers info.
        /// </summary>
        /// <returns>Known token providers.</returns>
        KnownTokenProviders GetKnownTokenProviders();
    }
}