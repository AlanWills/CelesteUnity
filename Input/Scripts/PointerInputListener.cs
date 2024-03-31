using Celeste.Events;
using UnityEngine;

namespace Celeste.Input
{
    [AddComponentMenu("Celeste/Input/Pointer Input Listener")]
    [RequireComponent(typeof(Collider2D))]
    public class PointerInputListener : MonoBehaviour, IEventListener<GameObject>
    {
        #region Properties and Fields

        public GameObjectEvent gameEvent;
        public GameObjectUnityEvent response;

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

        public void OnEventRaised(GameObject pointerInteractedGameObject)
        {
            if (pointerInteractedGameObject == gameObject)
            {
                response.Invoke(pointerInteractedGameObject);
            }
        }

        #endregion
    }
}
