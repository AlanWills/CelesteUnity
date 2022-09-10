using Celeste.Events;
using System;
using UnityEngine;

namespace Celeste.Parameters
{
    public class ParameterValue<T> : ScriptableObject, IValue<T> 
    {
        #region Properties and Fields

        protected virtual ParameterisedValueChangedEvent<T> OnValueChanged { get; }

        private T value;
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

                    var onValueChanged = OnValueChanged;
                    if (onValueChanged != null)
                    {
                        onValueChanged.Invoke(new ValueChangedArgs<T>(oldValue, value));
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

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            Value = defaultValue;
        }

        #endregion

        protected virtual T ConstrainValue(T input) { return input; }

        public void AddValueChangedCallback(Action<ValueChangedArgs<T>> callback)
        {
#if NULL_CHECKS
            Debug.Assert(OnValueChanged != null, $"Trying to add value changed callback to {name}, but it's {nameof(OnValueChanged)} event is null.");
            if (OnValueChanged != null)
#endif
            {
                OnValueChanged.AddListener(callback);
            }
        }

        public void RemoveValueChangedCallback(Action<ValueChangedArgs<T>> callback)
        {
#if NULL_CHECKS
            Debug.Assert(OnValueChanged != null, $"Trying to remove value changed callback from {name}, but it's {nameof(OnValueChanged)} event is null.");
            if (OnValueChanged != null)
#endif
            {
                OnValueChanged.RemoveListener(callback);
            }
        }

        public void RemoveAllValueChangedCallbacks()
        {
#if NULL_CHECKS
            Debug.Assert(OnValueChanged != null, $"Trying to remove all value changed callbacks from {name}, but it's {nameof(OnValueChanged)} event is null.");
            if (OnValueChanged != null)
#endif
            {
                OnValueChanged.RemoveAllListeners();
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
