using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(UIntValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Numeric/UInt Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class UIntValue : ParameterValue<uint, UIntValueChangedEvent>
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

        #region Equals & HashCode

        public override bool Equals(object obj)
        {
            return obj is UIntValue value &&
                   base.Equals(obj) &&
                   Value == value.Value;
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
