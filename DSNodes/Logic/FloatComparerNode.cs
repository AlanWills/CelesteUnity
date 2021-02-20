using Celeste.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XNode;
using static XNode.Node;

namespace Celeste.DS.Nodes.Logic
{
    [Serializable]
    [CreateNodeMenu("Celeste/Logic/Float Comparer")]
    [NodeTint(0.0f, 0.75f, 0.75f)]
    public class FloatComparerNode : DataNode
    {
        #region Properties and Fields

        [Input]
        public float lhs;

        public ConditionOperator condition;

        [Input]
        public float rhs;

        [Output]
        public bool result;

        #endregion

        #region Node Overrides

        public override object GetValue(NodePort port)
        {
            float lhsValue = GetInputValue("lhs", lhs);
            float rhsValue = GetInputValue("rhs", rhs);

            switch (condition)
            {
                case ConditionOperator.Equals:
                    return lhsValue == rhsValue;

                case ConditionOperator.NotEquals:
                    return lhsValue != rhsValue;

                case ConditionOperator.GreaterThan:
                    return lhsValue > rhsValue;

                case ConditionOperator.GreaterThanOrEqualTo:
                    return lhsValue >= rhsValue;

                case ConditionOperator.LessThan:
                    return lhsValue < rhsValue;

                case ConditionOperator.LessThanOrEqualTo:
                    return lhsValue <= rhsValue;

                default:
                    Debug.LogErrorFormat("Unhandled case {0} in IntComparerNode", condition);
                    return false;
            }
        }

        #endregion
    }
}
