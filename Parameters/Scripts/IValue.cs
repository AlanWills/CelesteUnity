using Celeste.Events;

namespace Celeste.Parameters
{
    public interface IValue<T>
    {
        T Value { get; set; }
    }
}