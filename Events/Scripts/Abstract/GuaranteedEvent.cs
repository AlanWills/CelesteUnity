using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class GuaranteedEvent : IEvent
    {
        #region Properties and Fields

        private Event BakedEvent
        {
            get
            {
                Debug.Assert(Application.isPlaying, $"Should only be accessing {nameof(BakedEvent)} whilst application is running.");
                if (bakedEvent == null)
                {
                    bakedEvent = ScriptableObject.CreateInstance<Event>();
                }

                return bakedEvent;
            }
        }

        [SerializeField] private Event bakedEvent;

        #endregion

        public ICallbackHandle AddListener(IEventListener listener)
        {
            return BakedEvent.AddListener(listener);
        }

        public ICallbackHandle AddListener(UnityAction unityAction)
        {
            return BakedEvent.AddListener(unityAction);
        }

        public void RemoveListener(UnityAction callback)
        {
            BakedEvent.RemoveListener(callback);
        }

        public void RemoveListener(IEventListener listener)
        {
            BakedEvent.RemoveListener(listener);
        }

        public void RemoveListener(ICallbackHandle callbackHandle)
        {
            BakedEvent.RemoveListener(callbackHandle);
        }

        public void Invoke()
        {
            BakedEvent.Invoke();
        }

        public void InvokeSilently()
        {
            BakedEvent.InvokeSilently();
        }
    }
}
