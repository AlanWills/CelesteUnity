using Celeste.Events;
using Celeste.Parameters.Constraints;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(IntValue), menuName = "Celeste/Parameters/Numeric/Int Value")]
    public class IntValue : ParameterValue<int>
    {
        #region Properties and Fields

        [SerializeField] private List<IntConstraint> valueConstraints = new List<IntConstraint>();

        #endregion

        protected override int ConstrainValue(int input)
        {
            return valueConstraints.Constrain(input);
        }

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

        #region Quality Overrides

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
