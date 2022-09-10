using Celeste.Parameters;
using System;
using UnityEngine;

namespace Celeste.FSM.Nodes.Parameters
{
    public enum SetMode
    {
        Absolute,
        Increment,
        Decrement
    }

    [Serializable]
    public abstract class SetValueNode<T, TValue, TReference> : FSMNode
        where TValue : IValue<T>
        where TReference : ParameterReference<T, TValue, TReference>
    {
        #region Properties and Fields

        public TValue value;
        [Input]
        public TReference newValue;

        #endregion

        #region Add/Remove/Copy

        protected override void OnAddToGraph()
        {
            base.OnAddToGraph();

            if (newValue == null)
            {
                newValue = CreateParameter<TReference>(name + "_newValue");
            }
        }

        protected override void OnRemoveFromGraph()
        {
            base.OnRemoveFromGraph();

            RemoveParameter(newValue);
        }

        protected override void OnCopyInGraph(FSMNode original)
        {
            base.OnCopyInGraph(original);

            SetValueNode<T, TValue, TReference> setValueNode = original as SetValueNode<T, TValue, TReference>;
            newValue = CreateParameter(setValueNode.newValue);
        }

        #endregion

        #region FSM Runtime Methods

        protected override void OnEnter()
        {
            base.OnEnter();

            Debug.AssertFormat(value != null, "Value is null in SetValueNode ({0})", graph.name);
            Debug.AssertFormat(newValue != null, "New Value is null in SetValueNode ({0})", graph.name);

            SetValue(newValue.IsConstant ? GetInputValue(nameof(newValue), newValue.Value) : newValue.Value);
        }

        protected abstract void SetValue(T newValue);

        #endregion
    }
}
