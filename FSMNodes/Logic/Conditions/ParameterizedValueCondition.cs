using Celeste.Logic;
using Celeste.Objects;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.FSM.Nodes.Logic.Conditions
{
    [Serializable]
    public abstract class ParameterizedValueCondition<T, TValue, TReference> : Condition
        where TValue : ParameterValue<T>
        where TReference : ParameterReference<T, TValue, TReference>
    {
        #region Properties and Fields

        public TReference value;
        public ConditionOperator condition;
        public bool useArgument = false;
        public TReference target;

        #endregion

        #region Init Methods

#if UNITY_EDITOR
        public override void Init_EditorOnly(IParameterContainer parameterContainer)
        {
            if (value == null)
            {
                value = parameterContainer.CreateParameter<TReference>(name + "_value");
            }

            if (target == null)
            {
                target = parameterContainer.CreateParameter<TReference>(name + "_target");
            }
        }

        public override void Cleanup_EditorOnly(IParameterContainer parameterContainer)
        {
            if (value != null)
            {
                parameterContainer.RemoveAsset(value);
            }

            if (target != null)
            {
                parameterContainer.RemoveAsset(target);
            }
        }
#endif

        #endregion

        #region Check Methods

        public sealed override bool Check(object arg)
        {
            if (useArgument)
            {
                target.IsConstant = true;
                target.Value = arg != null ? (T)arg : default;
            }

            return Check();
        }

        protected abstract bool Check();

        #endregion

        #region ICopyable

        public override void CopyFrom(Condition original)
        {
            ParameterizedValueCondition<T, TValue, TReference> valueCondition = original as ParameterizedValueCondition<T, TValue, TReference>;
            useArgument = valueCondition.useArgument;
            value.CopyFrom(valueCondition.value);
            condition = valueCondition.condition;
            target.CopyFrom(valueCondition.target);
        }

        #endregion
    }
}
