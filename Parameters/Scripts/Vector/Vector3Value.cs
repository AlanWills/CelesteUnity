using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(Vector3Value), menuName = "Celeste/Parameters/Vector/Vector3 Value")]
    public class Vector3Value : ParameterValue<Vector3, Vector3ValueChangedEvent>
    {
        #region Operators

        public static bool operator ==(Vector3Value value, Vector3 v)
        {
            return value.Value == v;
        }

        public static Vector3Value operator +(Vector3Value value, Vector3 v)
        {
            value.Value += v;
            return value;
        }

        public static Vector3Value operator -(Vector3Value value, Vector3 v)
        {
            value.Value -= v;
            return value;
        }

        public static bool operator !=(Vector3Value value, Vector3 v)
        {
            return value.Value != v;
        }

        #endregion

        #region Equals & HashCode

        public override bool Equals(object obj)
        {
            return obj is Vector3Value value &&
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
