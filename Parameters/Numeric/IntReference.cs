using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "IntReference", menuName = "Celeste/Parameters/Numeric/Int Reference")]
    public class IntReference : ParameterReference<int, IntValue, IntReference>
    {
        #region Operators

        public static bool operator ==(IntReference reference, int i)
        {
            return reference.Value == i;
        }

        public static IntReference operator +(IntReference reference, int i)
        {
            reference.Value += i;
            return reference;
        }

        public static IntReference operator -(IntReference reference, int i)
        {
            reference.Value -= i;
            return reference;
        }

        public static bool operator !=(IntReference reference, int i)
        {
            return reference.Value != i;
        }

        #endregion
    }
}
