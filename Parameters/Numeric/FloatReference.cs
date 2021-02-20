using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "FloatReference", menuName = "Celeste/Parameters/Numeric/Float Reference")]
    public class FloatReference : ParameterReference<float, FloatValue, FloatReference>
    {
        #region Operators

        public static bool operator ==(FloatReference reference, float f)
        {
            return reference.Value == f;
        }

        public static FloatReference operator +(FloatReference reference, float f)
        {
            reference.Value += f;
            return reference;
        }

        public static FloatReference operator -(FloatReference reference, float f)
        {
            reference.Value -= f;
            return reference;
        }

        public static bool operator !=(FloatReference reference, float f)
        {
            return reference.Value != f;
        }

        #endregion
    }
}
