using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "UIntReference", menuName = "Celeste/Parameters/Numeric/UInt Reference")]
    public class UIntReference : ParameterReference<uint, UIntValue, UIntReference>
    {
        #region Operators

        public static bool operator ==(UIntReference reference, uint ui)
        {
            return reference.Value == ui;
        }

        public static UIntReference operator +(UIntReference reference, uint ui)
        {
            reference.Value += ui;
            return reference;
        }

        public static UIntReference operator -(UIntReference reference, uint ui)
        {
            reference.Value -= ui;
            return reference;
        }

        public static bool operator !=(UIntReference reference, uint ui)
        {
            return reference.Value != ui;
        }

        #endregion
    }
}
