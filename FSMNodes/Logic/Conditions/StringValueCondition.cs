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
    [DisplayName("String")]
    public class StringValueCondition : ParameterizedValueCondition<string, StringValue, StringReference>
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

                default:
                    Debug.LogAssertionFormat("Condition Operator {0} is not supported in String Condition", condition);
                    return false;
            }
        }

        #endregion
    }
}
