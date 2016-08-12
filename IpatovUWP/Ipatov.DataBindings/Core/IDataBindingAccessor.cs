namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding access wrapper.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public interface IDataBindingAccessor<T> : IDataBindingValueGetter<T>, IDataBindingValueSetter<T>
    {         
    }
}