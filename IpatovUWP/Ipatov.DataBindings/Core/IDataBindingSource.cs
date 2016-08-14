namespace Ipatov.DataBindings
{
    /// <summary>
    /// Data binding source.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    public interface IDataBindingSource<out T> : IDataBindingValueGetter<T>, IDataBindingEventSource<T>
    {         
    }
}