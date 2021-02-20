using Celeste.Logic;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.FSM.Nodes.Logic.Conditions
{
    [Serializable]
    [DisplayName("Int")]
    public class IntValueCondition : ParameterizedValueCondition<int, IntValue, IntReference>
    {
        #region Condition Methods

        protected override bool Check()
        {
            switch (condition)
            {
                case ConditionOperator.Equals:
                    return value.Value == target.Value;

                case ConditionOperator.NotEquals:
                    return value.Value != target.Value;

                case ConditionOperator.LessThan:
                    return value.Value < target.Value;

                case ConditionOperator.LessThanOrEqualTo:
                    return value.Value <= target.Value;

                case ConditionOperator.GreaterThan:
                    return value.Value > target.Value;

                case ConditionOperator.GreaterThanOrEqualTo:
                    return value.Value >= target.Value;

                default:
                    Debug.LogAssertionFormat("Condition Operator {0} is not supported in Int Condition", condition);
                    return false;
            }
        }

        #endregion
    }
}
