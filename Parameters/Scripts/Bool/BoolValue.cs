using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(BoolValue), menuName = "Celeste/Parameters/Bool/Bool Value")]
    public class BoolValue : ParameterValue<bool, BoolValueChangedEvent>
    {
        #region Operators

        public static bool operator==(BoolValue value, bool b)
        {
            return value.Value == b;
        }

        public static bool operator !=(BoolValue value, bool b)
        {
            return value.Value != b;
        }

        #endregion

        #region Equals & HashCode

        public override bool Equals(object obj)
        {
            return obj is BoolValue value &&
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
