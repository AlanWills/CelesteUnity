using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Event Listener")]
    public class EventListener : MonoBehaviour, IEventListener
    {
        #region Properties and Fields

        public Event gameEvent;
        public UnityEvent response;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            gameEvent.AddListener(this);
        }

        private void OnDisable()
        {
            gameEvent.RemoveListener(this);
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
