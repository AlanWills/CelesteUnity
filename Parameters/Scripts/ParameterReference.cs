using Celeste.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [Serializable]
    public class ParameterReference<T, TValue, TReference> : ScriptableObject, ICopyable<TReference> 
                where TValue : ParameterValue<T>
                where TReference : ParameterReference<T, TValue, TReference>
    {
        #region Properties and Fields

        [SerializeField]
        private bool isConstant = true;
        public bool IsConstant
        {
            get { return isConstant; }
            set 
            { 
                isConstant = value; 

                if (isConstant)
                {
                    referenceValue = null;
                }
            }
        }

        [SerializeField]
        private T constantValue;

        [SerializeField]
        private TValue referenceValue;

        public T Value
        {
            get { return isConstant ? constantValue : referenceValue != null ? referenceValue.Value : default; }
            set
            {
                if (isConstant)
                {
                    constantValue = value;
                    referenceValue = null;
                }
                else
                {
                    referenceValue.Value = value;
                }
            }
        }

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
