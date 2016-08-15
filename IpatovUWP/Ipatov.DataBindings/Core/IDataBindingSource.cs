namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding source.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    /// <typeparam name="TSrc">Bound object type.</typeparam>
    public interface IDataBindingSource<out T, out TSrc> : IDataBindingValueGetter<T>, IDataBindingEventSource<TSrc>
    {         
    }
}