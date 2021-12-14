using System;
using UnityEngine;

namespace Celeste.Events
{
    [Serializable]
    public class ParameterisedEventRaiser<T, TEvent> : MonoBehaviour, IEventRaiser<T>
        where TEvent : ParameterisedEvent<T>
    {
        #region Properties and Fields

        public TEvent gameEvent;

        #endregion

        #region Response Methods

        public void Raise(T arguments)
        {
            gameEvent.Invoke(arguments);
        }

        #endregion
    }
}
