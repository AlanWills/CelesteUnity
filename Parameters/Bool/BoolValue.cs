using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "BoolValue", menuName = "Celeste/Parameters/Bool/Bool Value")]
    public class BoolValue : ParameterValue<bool>
    {
        #region Operators

        public static bool operator==(BoolValue value, bool b)
        {
            return value.Value == value;
        }

        public static bool operator !=(BoolValue value, bool b)
        {
            return value.Value != b;
        }

        #endregion
    }
}
