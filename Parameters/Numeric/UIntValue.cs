using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "UIntValue", menuName = "Celeste/Parameters/Numeric/UInt Value")]
    public class UIntValue : ParameterValue<uint>
    {
        #region Operators

        public static bool operator ==(UIntValue value, uint ui)
        {
            return value.Value == ui;
        }

        public static UIntValue operator +(UIntValue value, uint ui)
        {
            value.Value += ui;
            return value;
        }

        public static UIntValue operator -(UIntValue value, uint ui)
        {
            value.Value -= ui;
            return value;
        }

        public static bool operator !=(UIntValue value, uint ui)
        {
            return value.Value != ui;
        }

        #endregion
    }
}
