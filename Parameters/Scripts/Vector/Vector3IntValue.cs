using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "Vector3IntValue", menuName = "Celeste/Parameters/Vector/Vector3Int Value")]
    public class Vector3IntValue : ParameterValue<Vector3Int>
    {
        #region Properties and Fields

        [SerializeField] private Vector3IntEvent onValueChanged;
        protected override ParameterisedEvent<Vector3Int> OnValueChanged => onValueChanged;

        #endregion

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
