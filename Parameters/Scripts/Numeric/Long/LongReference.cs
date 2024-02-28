using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "LongReference", menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Numeric/Long Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
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

        #region

        public override bool Equals(object obj)
        {
            return obj is LongReference reference &&
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
