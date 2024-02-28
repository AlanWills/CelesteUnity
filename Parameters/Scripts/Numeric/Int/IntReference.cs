using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(IntReference), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Numeric/Int/Int Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
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

        #region

        public override bool Equals(object obj)
        {
            return obj is IntReference reference &&
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
