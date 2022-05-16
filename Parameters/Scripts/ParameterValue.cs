using Celeste.Events;
using Celeste.Parameters.Constraints;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Parameters
{
    public abstract class ParameterValue<T> : ScriptableObject, IValue<T>
    {
        #region Properties and Fields

        protected abstract ParameterisedEvent<T> OnValueChanged { get; }

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
                    this.value = value;

                    if (OnValueChanged != null)
                    {
                        OnValueChanged.InvokeSilently(this.value);
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

        [SerializeField] private T defaultValue;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            Value = defaultValue;
        }

        #endregion

        protected virtual T ConstrainValue(T input) { return input; }

        public void AddOnValueChangedCallback(Action<T> callback)
        {
            if (OnValueChanged != null)
            {
                OnValueChanged.AddListener(callback);
            }
        }

        public void RemoveOnValueChangedCallback(Action<T> callback)
        {
            if (OnValueChanged != null)
            {
                OnValueChanged.RemoveListener(callback);
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
