using System;
using System.Collections.Generic;
using UnityEngine;

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

    public class ParameterisedValueChangedEvent<T> : ScriptableObject, IEvent<ValueChangedArgs<T>>
    {
        #region Properties and Fields

#if UNITY_EDITOR
        [SerializeField, TextArea] private string helpText;
#endif

        private List<Action<ValueChangedArgs<T>>> gameEventListeners = new List<Action<ValueChangedArgs<T>>>();
        private List<Action<ValueChangedArgs<T>>> cachedListeners = new List<Action<ValueChangedArgs<T>>>();

        #endregion

        #region Event Management

        public void AddListener(IEventListener<ValueChangedArgs<T>> listener)
        {
            AddListener(listener.OnEventRaised);
        }

        public void AddListener(Action<ValueChangedArgs<T>> callback)
        {
            gameEventListeners.Add(callback);
        }

        public void RemoveListener(IEventListener<ValueChangedArgs<T>> listener)
        {
            RemoveListener(listener.OnEventRaised);
        }

        public void RemoveListener(Action<ValueChangedArgs<T>> callback)
        {
            gameEventListeners.Remove(callback);
        }

        public void RemoveAllListeners()
        {
            gameEventListeners.Clear();
        }

        public void Invoke(T oldValue, T newValue)
        {
            Invoke(new ValueChangedArgs<T>(oldValue, newValue));
        }

        public void Invoke(ValueChangedArgs<T> argument)
        {
            Debug.Log($"Event {name} was raised with argument {argument}.");
            InvokeSilently(argument);
        }

        public void InvokeSilently(T oldValue, T newValue)
        {
            Invoke(new ValueChangedArgs<T>(oldValue, newValue));
        }

        public void InvokeSilently(ValueChangedArgs<T> argument)
        {
            int gameEventListenersCount = gameEventListeners.Count;
            if (gameEventListenersCount > 0)
            {
                cachedListeners.Clear();
                cachedListeners.AddRange(gameEventListeners);

                // Cache the gameEventListeners to ensure that if events are unsubscribed from a callback
                // we can handle that and don't fall over
                for (int i = 0; i < gameEventListenersCount; ++i)
                {
                    Debug.Assert(cachedListeners[i] != null, $"Event {name} has a cached listener which is null.");
                    cachedListeners[i](argument);
                }

                cachedListeners.Clear();
            }
        }

        #endregion
    }
}
