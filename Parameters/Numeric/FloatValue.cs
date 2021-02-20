using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "FloatValue", menuName = "Celeste/Parameters/Numeric/Float Value")]
    public class FloatValue : ParameterValue<float>
    {
        #region Operators

        public static bool operator ==(FloatValue value, float f)
        {
            return value.Value == f;
        }

        public static FloatValue operator +(FloatValue value, float f)
        {
            value.Value += f;
            return value;
        }

        public static FloatValue operator -(FloatValue value, float f)
        {
            value.Value -= f;
            return value;
        }

        public static bool operator !=(FloatValue value, float f)
        {
            return value.Value != f;
        }

        #endregion
    }
}
