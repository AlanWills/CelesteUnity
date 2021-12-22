using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Events
{
    [Serializable]
    [CreateAssetMenu(fileName = "Event", menuName = "Celeste/Events/Event")]
    public class Event : ScriptableObject, IEvent
    {
        #region Properties and Fields

        private List<Action> gameEventListeners = new List<Action>();
        private List<Action> cachedListeners = new List<Action>();

        #endregion

        #region Event Management

        public void AddListener(IEventListener listener)
        {
            AddListener(listener.OnEventRaised);
        }

        public void AddListener(Action callback)
        {
            gameEventListeners.Add(callback);
        }

        public void RemoveListener(IEventListener listener)
        {
            RemoveListener(listener.OnEventRaised);
        }

        public void RemoveListener(Action callback)
        {
            gameEventListeners.Remove(callback);
        }

        public void RemoveAllListeners()
        {
            gameEventListeners.Clear();
        }

        public void Invoke()
        {
            Debug.Log($"Event {name} was raised");
            InvokeSilently();
        }

        public void InvokeSilently()
        {
            // Do the check for gameEventListeners to ensure that if events are unsubscribed from a callback
            // we can handle that and don't fall over
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
                    cachedListeners[i]();
                }

                cachedListeners.Clear();
            }
        }

        #endregion
    }
}
