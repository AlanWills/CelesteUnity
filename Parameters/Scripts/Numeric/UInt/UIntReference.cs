using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "UIntReference", menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Numeric/UInt Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
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

        #region Equals & HashCode

        public override bool Equals(object obj)
        {
            return obj is UIntReference reference &&
                   base.Equals(obj) &&
                   Value == reference.Value;
        }

        public override int GetHashCode()
        {
            int hashCode = -159790080;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            return hashCode;
        }

        #endregion
    }
}
