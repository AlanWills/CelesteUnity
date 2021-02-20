using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "IntValue", menuName = "Celeste/Parameters/Numeric/Int Value")]
    public class IntValue : ParameterValue<int>
    {
        #region Operators

        public static bool operator ==(IntValue value, int i)
        {
            return value.Value == i;
        }

        public static IntValue operator +(IntValue value, int i)
        {
            value.Value += i;
            return value;
        }

        public static IntValue operator -(IntValue value, int i)
        {
            value.Value -= i;
            return value;
        }

        public static bool operator !=(IntValue value, int i)
        {
            return value.Value != i;
        }

        #endregion
    }
}
