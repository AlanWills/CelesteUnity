using Celeste.Logic;
using System;
using UnityEngine;
using XNode;

namespace Celeste.DS.Nodes.Logic
{
    [Serializable]
    [CreateNodeMenu("Celeste/Logic/Bool Comparer")]
    [NodeTint(0.0f, 0.75f, 0.75f)]
    public class BoolComparerNode : DataNode
    {
        #region Properties and Fields

        [Input]
        public bool lhs;

        public ConditionOperator condition;

        [Input]
        public bool rhs;

        [Output]
        public bool result;

        #endregion

        #region Node Overrides

        public override object GetValue(NodePort port)
        {
            bool lhsValue = GetInputValue("lhs", lhs);
            bool rhsValue = GetInputValue("rhs", rhs);

            switch (condition)
            {
                case ConditionOperator.Equals:
                    return lhsValue == rhsValue;

                case ConditionOperator.NotEquals:
                    return lhsValue != rhsValue;

                default:
                    Debug.LogErrorFormat("Unhandled case {0} in BoolComparerNode", condition);
                    return false;
            }
        }

        #endregion
    }
}
