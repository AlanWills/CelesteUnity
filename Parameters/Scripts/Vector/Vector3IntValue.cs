using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(Vector3IntValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Vector/Vector3Int Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class Vector3IntValue : ParameterValue<Vector3Int, Vector3IntValueChangedEvent>
    {
        #region Operators

        public static bool operator ==(Vector3IntValue value, Vector3Int v)
        {
            return value.Value == v;
        }

        public static Vector3IntValue operator +(Vector3IntValue value, Vector3Int v)
        {
            value.Value += v;
            return value;
        }

        public static Vector3IntValue operator -(Vector3IntValue value, Vector3Int v)
        {
            value.Value -= v;
            return value;
        }

        public static bool operator !=(Vector3IntValue value, Vector3Int v)
        {
            return value.Value != v;
        }

        #endregion

        #region

        public override bool Equals(object obj)
        {
            return obj is Vector3IntValue value &&
                   base.Equals(obj) &&
                   Value.Equals(value.Value);
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
