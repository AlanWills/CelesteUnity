using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class GuaranteedEvent : IEvent
    {
        #region Properties and Fields

        private Event ObjectEvent
        {
            get
            {
                if (objectEvent == null)
                {
                    objectEvent = ScriptableObject.CreateInstance<Event>();
                }

                return objectEvent;
            }
        }

        [SerializeField] private Event objectEvent;

        #endregion

        public ICallbackHandle AddListener(IEventListener listener)
        {
            return ObjectEvent.AddListener(listener);
        }

        public ICallbackHandle AddListener(UnityAction unityAction)
        {
            return ObjectEvent.AddListener(unityAction);
        }

        public void RemoveListener(UnityAction callback)
        {
            ObjectEvent.RemoveListener(callback);
        }

        public void RemoveListener(IEventListener listener)
        {
            ObjectEvent.RemoveListener(listener);
        }

        public void RemoveListener(ICallbackHandle callbackHandle)
        {
            ObjectEvent.RemoveListener(callbackHandle);
        }

        public void Invoke()
        {
            ObjectEvent.Invoke();
        }

        public void InvokeSilently()
        {
            ObjectEvent.InvokeSilently();
        }
    }
}
