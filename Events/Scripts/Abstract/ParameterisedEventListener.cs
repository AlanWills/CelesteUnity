using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    public class ParameterisedEventListener<T, TEvent, TUnityEvent> : MonoBehaviour, IEventListener<T> 
                                                                      where TEvent : ParameterisedEvent<T>
                                                                      where TUnityEvent : UnityEvent<T>, new()
    {
        #region Properties and Fields

        public TEvent gameEvent;
        public bool logAddAndRemoveListener = false;
        public TUnityEvent response = new TUnityEvent();

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (logAddAndRemoveListener)
            {
                Debug.Log($"{name} is adding a listener to event {(gameEvent != null ? gameEvent.name : "null")}.");
            }

            Debug.Assert(gameEvent != null, $"{name} has a null game event on listener {GetType().Name}");
            gameEvent.AddListener(this);
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

        public void OnEventRaised(T arguments)
        {
            response.Invoke(arguments);
        }

        #endregion
    }
}
