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

        private List<IEventListener> gameEventListeners = new List<IEventListener>();

        #endregion

        #region Event Management

        public void AddEventListener(IEventListener listener)
        {
            gameEventListeners.Add(listener);
        }

        public void RemoveEventListener(IEventListener listener)
        {
            gameEventListeners.Remove(listener);
        }

        public void Raise()
        {
            Debug.Log(string.Format("Event {0} was raised", name));
            RaiseSilently();
        }

        public void RaiseSilently()
        {
            // Do the check for gameEventListeners to ensure that if events are unsubscribed from a callback
            // we can handle that and don't fall over
            for (int i = gameEventListeners.Count - 1; i >= 0 && gameEventListeners.Count > 0; --i)
            {
                gameEventListeners[i].OnEventRaised();
            }
        }

        #endregion
    }
}
