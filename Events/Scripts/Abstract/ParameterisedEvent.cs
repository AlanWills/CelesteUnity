using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Events
{
    public class ParameterisedEvent<T> : ScriptableObject, IEvent<T>
    {
        #region Properties and Fields

        private List<Action<T>> gameEventListeners = new List<Action<T>>();
        private List<Action<T>> cachedListeners = new List<Action<T>>();

        #endregion

        #region Event Management

        public void AddListener(IEventListener<T> listener)
        {
            AddListener(listener.OnEventRaised);
        }

        public void AddListener(Action<T> callback)
        {
            gameEventListeners.Add(callback);
        }

        public void RemoveListener(IEventListener<T> listener)
        {
            RemoveListener(listener.OnEventRaised);
        }

        public void RemoveListener(Action<T> callback)
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
