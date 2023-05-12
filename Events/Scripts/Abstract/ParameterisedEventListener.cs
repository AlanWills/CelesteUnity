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
        public TUnityEvent response = new TUnityEvent();

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            Debug.Assert(gameEvent != null, $"{name} has a null game event on listener {GetType().Name}");
            gameEvent.AddListener(this);
        }

        private void OnDisable()
        {
            Debug.Assert(gameEvent != null, $"{name} has a null game event on listener {GetType().Name}");
            gameEvent.RemoveListener(this);
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
