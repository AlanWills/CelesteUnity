using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    public struct ValueChangedArgs<T>
    {
        public T oldValue;
        public T newValue;

        public ValueChangedArgs(T oldValue, T newValue)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public override string ToString()
        {
            return $"{(oldValue != null ? oldValue.ToString() : "<null>")} - {(newValue != null ? newValue.ToString() : "<null>")}";
        }
    }

    public class ValueChangedUnityEvent<T> : UnityEvent<ValueChangedArgs<T>> { }

    public class ParameterisedValueChangedEvent<T> : ParameterisedEvent<ValueChangedArgs<T>>
    {
        #region Event Management

        public void Invoke(T oldValue, T newValue)
        {
            Invoke(new ValueChangedArgs<T>(oldValue, newValue));
        }

        public void InvokeSilently(T oldValue, T newValue)
        {
            InvokeSilently(new ValueChangedArgs<T>(oldValue, newValue));
        }

        #endregion
    }
}
