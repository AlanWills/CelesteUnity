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
    [DisplayName("Bool")]
    public class BoolValueCondition : ParameterizedValueCondition<bool, BoolValue, BoolReference>
    {
        #region Condition Methods

        protected override bool Check()
        {
            switch (condition)
            {
                case ConditionOperator.Equals:
                    Debug.LogFormat("{0} condition has value.Value: {1} and target.Value: {2}", name, value.Value, target.Value);
                    return value.Value == target.Value;

                case ConditionOperator.NotEquals:
                    return value.Value != target.Value;

                default:
                    Debug.LogAssertionFormat("Condition Operator {0} is not supported in Bool Condition", condition);
                    return false;
            }
        }

        #endregion
    }
}
