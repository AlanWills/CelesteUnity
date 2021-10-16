using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    public class ParameterisedEventListener<T, TEvent, TUnityEvent> : MonoBehaviour, IEventListener<T> 
                                                                                       where TEvent : ParameterisedEvent<T>
                                                                                       where TUnityEvent : UnityEvent<T>
    {
        #region Properties and Fields

        public TEvent gameEvent;
        public TUnityEvent response;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            Debug.Assert(gameEvent != null, string.Format("{0} has a null game event on listener {1}", name, GetType().Name));
            gameEvent.AddListener(this);
        }

        private void OnDisable()
        {
            Debug.Assert(gameEvent != null, string.Format("{0} has a null game event on listener {1}", name, GetType().Name));
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
