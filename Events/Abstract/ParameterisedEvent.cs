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

        private List<IEventListener<T>> gameEventListeners = new List<IEventListener<T>>();
        private List<IEventListener<T>> cachedListeners = new List<IEventListener<T>>();

        #endregion

        #region Event Management

        public void AddEventListener(IEventListener<T> listener)
        {
            gameEventListeners.Add(listener);
        }

        public void RemoveEventListener(IEventListener<T> listener)
        {
            gameEventListeners.Remove(listener);
        }

        public void Raise(T argument)
        {
            Debug.Log(string.Format("Event {0} was raised with argument {1}", name, argument.ToString()));
            RaiseSilently(argument);
        }

        public void RaiseSilently(T argument)
        {
            cachedListeners.Clear();
            cachedListeners.AddRange(gameEventListeners);

            // Cache the gameEventListeners to ensure that if events are unsubscribed from a callback
            // we can handle that and don't fall over
            for (int i = 0; i < cachedListeners.Count; ++i)
            {
                cachedListeners[i].OnEventRaised(argument);
            }

            cachedListeners.Clear();
        }

        #endregion
    }
}
