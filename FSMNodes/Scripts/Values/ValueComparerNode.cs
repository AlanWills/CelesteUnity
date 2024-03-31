using Celeste.Logic;
using Celeste.Parameters;
using System;
using UnityEngine;

namespace Celeste.FSM.Nodes
{
    public abstract class ValueComparerNode<T, TValue, TReference> : FSMNode
        where T : IEquatable<T>
        where TValue : IValue<T>
        where TReference : ParameterReference<T, TValue, TReference>
    {
        #region Properties and Fields

        [Input, SerializeField] private T inputValue;
        [SerializeField] private ConditionOperator condition;
        [SerializeField] private TReference targetValue;

        private const string TRUE_PORT_NAME = "True";
        private const string FALSE_PORT_NAME = "False";

        #endregion

        #region FSM Runtime

        protected override void OnAddToGraph()
        {
            base.OnAddToGraph();

            if (targetValue == null)
            {
                targetValue = CreateParameter<TReference>($"{name}_TargetValue");
                targetValue.IsConstant = true;
            }

            AddOutputPort(TRUE_PORT_NAME);
            AddOutputPort(FALSE_PORT_NAME);
            RemoveDefaultOutputPort();
        }

        protected override FSMNode OnUpdate()
        {
            T input = GetInputValue<T>(nameof(inputValue));
            return input.SatisfiesEquality(condition, targetValue.Value) ? GetConnectedNode(TRUE_PORT_NAME) : GetConnectedNode(FALSE_PORT_NAME);
        }

        #endregion
    }
}
