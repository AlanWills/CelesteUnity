using Celeste.Objects;
using System;
using UnityEngine;

namespace Celeste.Parameters
{
    [Serializable]
    public class ParameterReference<T, TValue, TReference> : ScriptableObject, /*IValue<T>, */ICopyable<TReference> 
                where TValue : IValue<T>
                where TReference : ParameterReference<T, TValue, TReference>
    {
        #region Properties and Fields

        public bool IsConstant
        {
            get { return isConstant; }
            set 
            { 
                isConstant = value; 

                if (isConstant)
                {
                    referenceValue = default;
                }
            }
        }
        
        public T Value
        {
            get { return isConstant ? constantValue : referenceValue != null ? referenceValue.Value : default; }
            set
            {
                if (isConstant)
                {
                    constantValue = value;
                    referenceValue = default;
                }
                else
                {
                    referenceValue.Value = value;
                }
            }
        }

        public TValue ReferenceValue
        {
            get { return referenceValue; }
            set
            {
                Debug.Assert(!isConstant, $"Could not set reference value on constant reference {name}.  Ignoring...");
                if (!isConstant)
                {
                    referenceValue = value;
                }
            }
        }

#if UNITY_EDITOR
        [SerializeField, TextArea] private string helpText;
#endif
        [SerializeField] private bool isConstant = true;
        [SerializeField] private T constantValue;
        [SerializeField] private TValue referenceValue;

        #endregion

        #region Copy Methods

        public void CopyFrom(TReference otherParameter)
        {
            isConstant = otherParameter.isConstant;
            constantValue = otherParameter.constantValue;
            referenceValue = otherParameter.referenceValue;
        }

        #endregion

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
