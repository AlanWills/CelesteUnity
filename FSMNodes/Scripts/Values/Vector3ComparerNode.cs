using Celeste.Logic;
using Celeste.Parameters;
using Celeste.Tools;
using System;
using UnityEngine;

namespace Celeste.FSM.Nodes
{
    [Serializable]
    [CreateNodeMenu("Celeste/Values/Vector3 Equals")]
    [NodeWidth(300)]
    public class Vector3ComparerNode : FSMNode
    {
        #region Properties and Fields

        [Input, SerializeField] private Vector3 inputValue;
        [SerializeField] private ConditionOperator condition;
        [SerializeField] private Vector3Reference targetValue;

        private const string TRUE_PORT_NAME = "True";
        private const string FALSE_PORT_NAME = "False";

        #endregion

        #region FSM Runtime

        protected override void OnAddToGraph()
        {
            base.OnAddToGraph();

            if (targetValue == null)
            {
                targetValue = CreateParameter<Vector3Reference>($"{name}_TargetValue");
                targetValue.IsConstant = true;
            }

            AddOutputPort(TRUE_PORT_NAME);
            AddOutputPort(FALSE_PORT_NAME);
            RemoveDefaultOutputPort();
        }

        protected override FSMNode OnUpdate()
        {
            Vector3 input = GetInputValue<Vector3>(nameof(inputValue));
            return input.SatisfiesEquality(condition, targetValue.Value) ? GetConnectedNode(TRUE_PORT_NAME) : GetConnectedNode(FALSE_PORT_NAME);
        }

        #endregion
    }
}
