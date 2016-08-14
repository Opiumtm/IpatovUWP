namespace Ipatov.DataBindings
{
    /// <summary>
    /// Marker interface for wrapped binding event sources.
    /// </summary>
    /// <typeparam name="T">Bound object type.</typeparam>
    public interface IDataBindingEventSourceWrapper<out T> : IDataBindingEventSource<T>
    {        
    }
}