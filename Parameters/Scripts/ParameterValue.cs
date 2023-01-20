using Celeste.Events;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Parameters
{
    public class ParameterValue<T, TValueChangedEvent> : ScriptableObject, IValue<T> 
        where TValueChangedEvent : ParameterisedValueChangedEvent<T>
    {
        #region Properties and Fields

        [NonSerialized] private UnityEvent<ValueChangedArgs<T>> onValueChangedUnityEvent;
        private UnityEvent<ValueChangedArgs<T>> OnValueChangedChangeUnityEvent
        {
            get
            {
                if (onValueChangedUnityEvent == null)
                {
                    onValueChangedUnityEvent = new UnityEvent<ValueChangedArgs<T>>();
                }

                return onValueChangedUnityEvent;
            }
        }

        [NonSerialized] private T value;
        public T Value 
        {
            get { return Application.isPlaying ? value : DefaultValue; }
            set
            {
                if (this.value == null && value == null)
                {
                    return;
                }

                value = ConstrainValue(value);

                if ((this.value == null && value != null) ||
                    !this.value.Equals(value))
                {
                    T oldValue = this.value;
                    this.value = value;

                    ValueChangedArgs<T> valueChangedArgs = new ValueChangedArgs<T>(oldValue, value);

                    if (onValueChanged != null)
                    {
                        onValueChanged.Invoke(valueChangedArgs);
                    }
                    else
                    {
                        OnValueChangedChangeUnityEvent.Invoke(valueChangedArgs);
                    }
                }
            }
        }

        public T DefaultValue
        {
            get { return defaultValue; }
            set
            {
#if UNITY_EDITOR
                T oldValue = defaultValue;
#endif
                defaultValue = value;

#if UNITY_EDITOR
                if (!oldValue.Equals(value))
                {
                    UnityEditor.EditorUtility.SetDirty(this);
                }
#endif
            }
        }

#if UNITY_EDITOR
        [SerializeField, TextArea] private string helpText;
#endif
        [SerializeField] private T defaultValue;
        [SerializeField] private TValueChangedEvent onValueChanged;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            Value = defaultValue;
        }

        #endregion

        protected virtual T ConstrainValue(T input) { return input; }

        public void AddValueChangedCallback(UnityAction<ValueChangedArgs<T>> callback)
        {
            if (onValueChanged != null)
            {
                onValueChanged.AddListener(callback);
            }
            else
            {
                OnValueChangedChangeUnityEvent.AddListener(callback);
            }
        }

        public void RemoveValueChangedCallback(UnityAction<ValueChangedArgs<T>> callback)
        {
            if (onValueChanged != null)
            {
                onValueChanged.RemoveListener(callback);
            }
            else
            {
                OnValueChangedChangeUnityEvent.RemoveListener(callback);
            }
        }

        public void RemoveAllValueChangedCallbacks()
        {
            if (onValueChanged != null)
            {
                onValueChanged.RemoveAllListeners();
            }
            else
            {
                OnValueChangedChangeUnityEvent.RemoveAllListeners();
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
