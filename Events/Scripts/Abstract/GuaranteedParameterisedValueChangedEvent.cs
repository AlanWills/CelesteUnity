using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class GuaranteedParameterisedValueChangedEvent<TEvent, TParam> : IEvent<ValueChangedArgs<TParam>> where TEvent : ParameterisedEvent<ValueChangedArgs<TParam>>
    {
        #region Properties and Fields

        private TEvent BakedEvent
        {
            get
            {
                Debug.Assert(Application.isPlaying, $"Should only be accessing {nameof(BakedEvent)} whilst application is running.");
                if (bakedEvent == null)
                {
                    bakedEvent = ScriptableObject.CreateInstance<TEvent>();
                }

                return bakedEvent;
            }
        }

        [SerializeField] private TEvent bakedEvent;

        #endregion

        public ICallbackHandle AddListener(IEventListener<ValueChangedArgs<TParam>> listener)
        {
            return BakedEvent.AddListener(listener);
        }

        public ICallbackHandle AddListener(UnityAction<ValueChangedArgs<TParam>> unityAction)
        {
            return BakedEvent.AddListener(unityAction);
        }

        public void RemoveListener(UnityAction<ValueChangedArgs<TParam>> callback)
        {
            BakedEvent.RemoveListener(callback);
        }

        public void RemoveListener(IEventListener<ValueChangedArgs<TParam>> listener)
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

        public void Invoke(TParam oldValue, TParam newValue)
        {
            BakedEvent.Invoke(new ValueChangedArgs<TParam>(oldValue, newValue));
        }

        public void Invoke(ValueChangedArgs<TParam> param)
        {
            BakedEvent.Invoke(param);
        }

        public void InvokeSilently(TParam oldValue, TParam newValue)
        {
            BakedEvent.InvokeSilently(new ValueChangedArgs<TParam>(oldValue, newValue));
        }

        public void InvokeSilently(ValueChangedArgs<TParam> param)
        {
            BakedEvent.InvokeSilently(param);
        }
    }
}
