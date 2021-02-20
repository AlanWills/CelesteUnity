using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "LongValue", menuName = "Celeste/Parameters/Numeric/Long Value")]
    public class LongValue : ParameterValue<long>
    {
        #region Operators

        public static bool operator ==(LongValue value, long l)
        {
            return value.Value == l;
        }

        public static LongValue operator +(LongValue value, long l)
        {
            value.Value += l;
            return value;
        }

        public static LongValue operator -(LongValue value, long l)
        {
            value.Value -= l;
            return value;
        }

        public static bool operator !=(LongValue value, long l)
        {
            return value.Value != l;
        }

        #endregion
    }
}
