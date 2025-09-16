using Celeste.Events;
using Celeste.Tools.Attributes.GUI;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Parameters
{
    public class ParameterValue<T, TValueChangedEvent> : ScriptableObject, IValue<T> 
        where TValueChangedEvent : ParameterisedValueChangedEvent<T>
    {
        #region Properties and Fields

        private TValueChangedEvent OnValueChangedEvent
        {
            get
            {
                if (onValueChanged == null && fallbackOnValueChanged == null)
                {
                    // Fallback events are always invoked silently to prevent useless log spam from unnamed events
                    fallbackOnValueChanged = CreateInstance<TValueChangedEvent>();
                    fallbackOnValueChanged.name = $"{name}_OnValueChanged_Fallback";
                }

                return onValueChanged ?? fallbackOnValueChanged;
            }
        }

        [NonSerialized] private T value;
        public T Value 
        {
            get => Application.isPlaying ? value : DefaultValue;
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

                    if (invokeValueChangedSilently)
                    {
                        OnValueChangedEvent.InvokeSilently(oldValue, value);
                    }
                    else
                    {
                        OnValueChangedEvent.Invoke(oldValue, value);
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
        [SerializeField] private bool invokeValueChangedSilently = true;

        [NonSerialized] private TValueChangedEvent fallbackOnValueChanged;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            value = defaultValue;
        }

        #endregion

        protected virtual T ConstrainValue(T input) { return input; }

        public void AddValueChangedCallback(UnityAction<ValueChangedArgs<T>> callback)
        {
            OnValueChangedEvent.AddListener(callback);
        }

        public void RemoveValueChangedCallback(UnityAction<ValueChangedArgs<T>> callback)
        {
            OnValueChangedEvent.RemoveListener(callback);
        }

        public void RemoveAllValueChangedCallbacks()
        {
            OnValueChangedEvent.RemoveAllListeners();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
