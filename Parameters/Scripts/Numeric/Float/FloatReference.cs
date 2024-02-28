using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(FloatReference), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Numeric/Float/Float Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class FloatReference : ParameterReference<float, FloatValue, FloatReference>
    {
        #region Operators

        public static bool operator ==(FloatReference reference, float f)
        {
            return reference.Value == f;
        }

        public static FloatReference operator +(FloatReference reference, float f)
        {
            reference.Value += f;
            return reference;
        }

        public static FloatReference operator -(FloatReference reference, float f)
        {
            reference.Value -= f;
            return reference;
        }

        public static bool operator !=(FloatReference reference, float f)
        {
            return reference.Value != f;
        }

        #endregion

        #region Equals & HashCode

        public override bool Equals(object obj)
        {
            return obj is FloatReference reference &&
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
