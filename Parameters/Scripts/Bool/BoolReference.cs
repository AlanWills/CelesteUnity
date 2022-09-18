using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(BoolReference), menuName = "Celeste/Parameters/Bool/Bool Reference")]
    public class BoolReference : ParameterReference<bool, BoolValue, BoolReference>
    {
        #region Operators

        public static bool operator ==(BoolReference reference, bool b)
        {
            return reference.Value == b;
        }

        public static bool operator !=(BoolReference reference, bool b)
        {
            return reference.Value != b;
        }

        #endregion

        #region Equals & HashCode

        public override bool Equals(object obj)
        {
            return obj is BoolReference reference &&
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
