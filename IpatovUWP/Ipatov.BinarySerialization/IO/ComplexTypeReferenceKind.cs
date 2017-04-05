namespace Ipatov.BinarySerialization.IO
{
    /// <summary>
    /// Complex type reference kind.
    /// </summary>
    internal enum ComplexTypeReferenceKind : byte
    {
        Unknown = 0,
        String = 1,
        ComplexType = 2,
        ComplexTypeReference = 3,
        ByteArray = 4,
    }
}