using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Raisers/Event Raiser")]
    public class EventRaiser : MonoBehaviour, IEventRaiser
    {
        #region Properties and Fields

        public Event gameEvent;

        #endregion

        #region Response Methods

        public void Raise()
        {
            gameEvent.Invoke();
        }

        #endregion
    }
}
