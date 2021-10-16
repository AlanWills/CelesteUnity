using Celeste.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Input
{
    [AddComponentMenu("Celeste/Input/Input Listener")]
    [RequireComponent(typeof(Collider2D))]
    public class InputListener : MonoBehaviour, IEventListener<GameObjectClickEventArgs>
    {
        #region Properties and Fields

        public GameObjectClickEvent gameEvent;
        public Vector3UnityEvent response;

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

        public void OnEventRaised(GameObjectClickEventArgs clickedGameObjectArgs)
        {
            if (clickedGameObjectArgs.gameObject == gameObject)
            {
                response.Invoke(clickedGameObjectArgs.clickWorldPosition);
            }
        }

        #endregion
    }
}
