using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Event Listener")]
    public class EventListener : MonoBehaviour, IEventListener
    {
        #region Properties and Fields

        public Event gameEvent;
        public bool logAddAndRemoveListener = false;
        public UnityEvent response;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (logAddAndRemoveListener)
            {
                Debug.Log($"{name} is adding a listener to event {(gameEvent != null ? gameEvent.name : "null")}.");
            }

            Debug.Assert(gameEvent != null, $"{name} has a null game event on listener {GetType().Name}");
            gameEvent?.AddListener(this);
        }

        private void OnDisable()
        {
            if (logAddAndRemoveListener)
            {
                Debug.Log($"{name} is removing a listener to event {(gameEvent != null ? gameEvent.name : "null")}.");
            }
            
            gameEvent?.RemoveListener(this);
        }

        #endregion

        #region Response Methods

        public void OnEventRaised()
        {
            response.Invoke();
        }

        #endregion
    }
}
