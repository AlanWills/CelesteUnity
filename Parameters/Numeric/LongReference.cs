using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "LongReference", menuName = "Celeste/Parameters/Numeric/Long Reference")]
    public class LongReference : ParameterReference<long, LongValue, LongReference>
    {
        #region Operators

        public static bool operator ==(LongReference reference, long l)
        {
            return reference.Value == l;
        }

        public static LongReference operator +(LongReference reference, long l)
        {
            reference.Value += l;
            return reference;
        }

        public static LongReference operator -(LongReference reference, long l)
        {
            reference.Value -= l;
            return reference;
        }

        public static bool operator !=(LongReference reference, long l)
        {
            return reference.Value != l;
        }

        #endregion
    }
}
