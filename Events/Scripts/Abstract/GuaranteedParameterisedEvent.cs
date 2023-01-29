using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class GuaranteedParameterisedEvent<TEvent, TParam> : IEvent<TParam> where TEvent : ParameterisedEvent<TParam>
    {
        #region Properties and Fields

        private TEvent ObjectEvent
        {
            get
            {
                if (objectEvent == null)
                {
                    objectEvent = ScriptableObject.CreateInstance<TEvent>();
                }

                return objectEvent;
            }
        }

        [SerializeField] private TEvent objectEvent;

        #endregion

        public ICallbackHandle AddListener(IEventListener<TParam> listener)
        {
            return ObjectEvent.AddListener(listener);
        }

        public ICallbackHandle AddListener(UnityAction<TParam> unityAction)
        {
            return ObjectEvent.AddListener(unityAction);
        }

        public void RemoveListener(UnityAction<TParam> callback)
        {
            ObjectEvent.RemoveListener(callback);
        }

        public void RemoveListener(IEventListener<TParam> listener)
        {
            ObjectEvent.RemoveListener(listener);
        }

        public void RemoveListener(ICallbackHandle callbackHandle)
        {
            ObjectEvent.RemoveListener(callbackHandle);
        }

        public void Invoke(TParam param)
        {
            ObjectEvent.Invoke(param);
        }

        public void InvokeSilently(TParam param)
        {
            ObjectEvent.InvokeSilently(param);
        }
    }
}
