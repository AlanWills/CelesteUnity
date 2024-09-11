using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class GuaranteedParameterisedValueChangedEvent<TEvent, TParam> : IEvent<ValueChangedArgs<TParam>> where TEvent : ParameterisedEvent<ValueChangedArgs<TParam>>
    {
        #region Properties and Fields

        private TEvent EventImpl
        {
            get
            {
                if (runtimeEvent == null && Application.isPlaying)
                {
                    runtimeEvent = bakedEvent ?? ScriptableObject.CreateInstance<TEvent>();
                    runtimeEvent.name = bakedEvent != null ? bakedEvent.name : $"Runtime{typeof(TEvent).Name}";
                }

                return runtimeEvent;
            }
        }

        [SerializeField] private TEvent bakedEvent;

        [NonSerialized] private TEvent runtimeEvent;

        #endregion

        public ICallbackHandle AddListener(IEventListener<ValueChangedArgs<TParam>> listener)
        {
            return EventImpl.AddListener(listener);
        }

        public ICallbackHandle AddListener(UnityAction<ValueChangedArgs<TParam>> unityAction)
        {
            return EventImpl.AddListener(unityAction);
        }

        public void RemoveListener(UnityAction<ValueChangedArgs<TParam>> callback)
        {
            EventImpl.RemoveListener(callback);
        }

        public void RemoveListener(IEventListener<ValueChangedArgs<TParam>> listener)
        {
            EventImpl.RemoveListener(listener);
        }

        public void RemoveListener(ICallbackHandle callbackHandle)
        {
            EventImpl.RemoveListener(callbackHandle);
        }

        public void RemoveAllListeners()
        {
            EventImpl.RemoveAllListeners();
        }

        public void Invoke(TParam oldValue, TParam newValue)
        {
            EventImpl.Invoke(new ValueChangedArgs<TParam>(oldValue, newValue));
        }

        public void Invoke(ValueChangedArgs<TParam> param)
        {
            EventImpl.Invoke(param);
        }

        public void InvokeSilently(TParam oldValue, TParam newValue)
        {
            EventImpl.InvokeSilently(new ValueChangedArgs<TParam>(oldValue, newValue));
        }

        public void InvokeSilently(ValueChangedArgs<TParam> param)
        {
            EventImpl.InvokeSilently(param);
        }
    }
}
