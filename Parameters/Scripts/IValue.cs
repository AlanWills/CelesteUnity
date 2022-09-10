using Celeste.Events;
using System;

namespace Celeste.Parameters
{
    public interface IValue<T>
    {
        T Value { get; set; }

        void AddValueChangedCallback(Action<ValueChangedArgs<T>> args);
        void RemoveValueChangedCallback(Action<ValueChangedArgs<T>> args);
    }
}