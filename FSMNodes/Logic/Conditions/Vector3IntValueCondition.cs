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
    [DisplayName("Vector3 Int")]
    public class Vector3IntValueCondition : ParameterizedValueCondition<Vector3Int, Vector3IntValue, Vector3IntReference>
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
                    Debug.LogAssertionFormat("Condition Operator {0} is not supported in Vector3Int Condition", condition);
                    return false;
            }
        }

        #endregion
    }
}
