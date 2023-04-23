using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class GuaranteedParameterisedEvent<TEvent, TParam> : IEvent<TParam> where TEvent : ParameterisedEvent<TParam>
    {
        #region Properties and Fields

        private TEvent BakedEvent
        {
            get
            {
                if (bakedEvent == null)
                {
                    fallbackRuntimeEvent = fallbackRuntimeEvent == null ? ScriptableObject.CreateInstance<TEvent>() : fallbackRuntimeEvent;
                    return fallbackRuntimeEvent;
                }

                return bakedEvent;
            }
        }

        [SerializeField] private TEvent bakedEvent;

        [NonSerialized] private TEvent fallbackRuntimeEvent;

        #endregion

        public ICallbackHandle AddListener(IEventListener<TParam> listener)
        {
            return BakedEvent.AddListener(listener);
        }

        public ICallbackHandle AddListener(UnityAction<TParam> unityAction)
        {
            return BakedEvent.AddListener(unityAction);
        }

        public void RemoveListener(UnityAction<TParam> callback)
        {
            BakedEvent.RemoveListener(callback);
        }

        public void RemoveListener(IEventListener<TParam> listener)
        {
            BakedEvent.RemoveListener(listener);
        }

        public void RemoveListener(ICallbackHandle callbackHandle)
        {
            BakedEvent.RemoveListener(callbackHandle);
        }

        public void RemoveAllListeners()
        {
            BakedEvent.RemoveAllListeners();
        }

        public void Invoke(TParam param)
        {
            BakedEvent.Invoke(param);
        }

        public void InvokeSilently(TParam param)
        {
            BakedEvent.InvokeSilently(param);
        }
    }
}
