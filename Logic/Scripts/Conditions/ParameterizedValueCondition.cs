using Celeste.Logic;
using Celeste.Parameters;
using System;

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

        public override void Init()
        {
            if (value == null)
            {
                value = CreateDependentAsset<TReference>($"{name}_value");
            }

            if (target == null)
            {
                target = CreateDependentAsset<TReference>($"{name}_target");
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
    }
}
