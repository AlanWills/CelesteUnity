using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Parameters
{
    public struct ValueChangedArgs<T>
    {
        public T oldValue;
        public T newValue;

        public ValueChangedArgs(T oldValue, T newValue)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }
    }

    public class ParameterValue<T> : ScriptableObject, IValue<T>
    {
        #region Properties and Fields

        protected UnityEvent<ValueChangedArgs<T>> OnValueChanged { get; } = new UnityEvent<ValueChangedArgs<T>>();

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

                    OnValueChanged.Invoke(new ValueChangedArgs<T>(oldValue, value));
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

        public void AddValueChangedCallback(UnityAction<ValueChangedArgs<T>> callback)
        {
            OnValueChanged.AddListener(callback);
        }

        public void RemoveValueChangedCallback(UnityAction<ValueChangedArgs<T>> callback)
        {
            OnValueChanged.RemoveListener(callback);
        }

        public void RemoveAllValueChangedCallbacks()
        {
            OnValueChanged.RemoveAllListeners();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
