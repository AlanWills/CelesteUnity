using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    public class ParameterisedEvent<T> : ScriptableObject, IEvent<T>
    {
        #region Properties and Fields

#if UNITY_EDITOR
        [SerializeField, TextArea] private string helpText;
#endif

        private List<UnityAction<T>> gameEventListeners = new List<UnityAction<T>>();
        private List<UnityAction<T>> cachedListeners = new List<UnityAction<T>>();

        #endregion

        #region Event Management

        public void AddListener(IEventListener<T> listener)
        {
            AddListener(listener.OnEventRaised);
        }

        public void AddListener(UnityAction<T> callback)
        {
            gameEventListeners.Add(callback);
        }

        public void RemoveListener(IEventListener<T> listener)
        {
            RemoveListener(listener.OnEventRaised);
        }

        public void RemoveListener(UnityAction<T> callback)
        {
            gameEventListeners.Remove(callback);
        }

        public void RemoveAllListeners()
        {
            gameEventListeners.Clear();
        }

        public void Invoke(T argument)
        {
            Debug.Log($"Event {name} was raised with argument {(argument != null ? argument.ToString() : "<null>")}");
            InvokeSilently(argument);
        }

        public void InvokeSilently(T argument)
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
