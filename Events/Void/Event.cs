using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Events
{
    [Serializable]
    [CreateAssetMenu(fileName = "Event", menuName = "Celeste/Events/Event")]
    public class Event : ScriptableObject, IEvent
    {
        #region Properties and Fields

        private List<Action> gameEventListeners = new List<Action>();

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
            Debug.Log(string.Format("Event {0} was raised", name));
            InvokeSilently();
        }

        public void InvokeSilently()
        {
            // Do the check for gameEventListeners to ensure that if events are unsubscribed from a callback
            // we can handle that and don't fall over
            for (int i = gameEventListeners.Count - 1; i >= 0 && gameEventListeners.Count > 0; --i)
            {
                gameEventListeners[i]();
            }
        }

        #endregion
    }
}
