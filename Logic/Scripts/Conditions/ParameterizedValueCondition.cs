using Celeste.Events;
using Celeste.Parameters;
using System;
using UnityEngine.Events;

namespace Celeste.Logic
{
    [Serializable]
    public abstract class ParameterizedValueCondition<T, TValue, TReference> : Condition
        where TValue : ParameterValue<T>
        where TReference : ParameterReference<T, TValue, TReference>
    {
        #region Properties and Fields

        public TReference value;
        public ConditionOperator condition;
        public TReference target;

        #endregion

        protected override void DoInit()
        {
            if (value == null)
            {
                value = CreateDependentAsset<TReference>($"{name}_value");
            }

            if (target == null)
            {
                target = CreateDependentAsset<TReference>($"{name}_target");
            }

            if (!value.IsConstant)
            {
                value.ReferenceValue.AddValueChangedCallback(OnValueChangedCallback);
            }

            if (!target.IsConstant)
            {
                target.ReferenceValue.AddValueChangedCallback(OnTargetChangedCallback);
            }
        }

        protected override void DoShutdown()
        {
            if (!value.IsConstant)
            {
                value.ReferenceValue.RemoveValueChangedCallback(OnValueChangedCallback);
            }

            if (!target.IsConstant)
            {
                target.ReferenceValue.RemoveValueChangedCallback(OnTargetChangedCallback);
            }
        }

        #region Check Methods

        public override void SetTarget(object arg)
        {
            target.IsConstant = true;
            target.Value = arg != null ? (T)arg : default;
        }

        #endregion

        #region ICopyable

        public override void CopyFrom(Condition original)
        {
            ParameterizedValueCondition<T, TValue, TReference> valueCondition = original as ParameterizedValueCondition<T, TValue, TReference>;
            value.CopyFrom(valueCondition.value);
            condition = valueCondition.condition;
            target.CopyFrom(valueCondition.target);
        }

        #endregion

        #region Callbacks

        private void OnValueChangedCallback(ValueChangedArgs<T> args)
        {
            Check();
        }

        private void OnTargetChangedCallback(ValueChangedArgs<T> args)
        {
            Check();
        }

        #endregion
    }
}
