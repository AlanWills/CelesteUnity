using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "IntValue", menuName = "Celeste/Parameters/Numeric/Int Value")]
    public class IntValue : ParameterValue<int>
    {
        #region Properties and Fields

        [SerializeField] private IntEvent onValueChanged;
        protected override ParameterisedEvent<int> OnValueChanged => onValueChanged;

        #endregion

        #region Operators

        public static bool operator ==(IntValue value, int i)
        {
            return value.Value == i;
        }

        public static IntValue operator +(IntValue value, int i)
        {
            value.Value += i;
            return value;
        }

        public static IntValue operator -(IntValue value, int i)
        {
            value.Value -= i;
            return value;
        }

        public static bool operator !=(IntValue value, int i)
        {
            return value.Value != i;
        }

        #endregion

        #region

        public override bool Equals(object obj)
        {
            return obj is IntValue value &&
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
