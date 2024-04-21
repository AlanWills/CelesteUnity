using Celeste.Events;
using UnityEngine.Events;

namespace Celeste.Parameters
{
    public interface IValue<T>
    {
        T Value { get; set; }
        T DefaultValue { get; set; }

        void AddValueChangedCallback(UnityAction<ValueChangedArgs<T>> args);
        void RemoveValueChangedCallback(UnityAction<ValueChangedArgs<T>> args);
    }
}