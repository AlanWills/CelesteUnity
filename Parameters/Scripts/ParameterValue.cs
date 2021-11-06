using System;
using UnityEngine;

namespace Celeste.Parameters
{
    [Serializable]
    public class ParameterValue<T> : ScriptableObject, IValue<T>
    {
        #region Properties and Fields

        public T Value { get; set; }

        [SerializeField]
        private T defaultValue;
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

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            Value = defaultValue;
        }

        #endregion

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
