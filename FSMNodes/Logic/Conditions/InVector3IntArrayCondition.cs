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
    [DisplayName("In Vector3 Int Array")]
    public class InVector3IntArrayCondition : Condition
    {
        #region Properties and Fields

        public bool useArgument = false;
        public Vector3IntArrayValue value;
        public ConditionOperator condition;
        public Vector3IntReference target;

        #endregion

        #region Init Methods

#if UNITY_EDITOR
        public override void Init_EditorOnly(IParameterContainer parameterContainer)
        {
            if (target == null)
            {
                target = parameterContainer.CreateParameter<Vector3IntReference>(name + "_value");
            }
        }

        public override void Cleanup_EditorOnly(IParameterContainer parameterContainer)
        {
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
                target.Value = arg != null ? (Vector3Int)arg : default;
            }

            return Check();
        }

        private bool Check()
        {
            switch (condition)
            {
                case ConditionOperator.Equals:
                    return value.Value.Contains(target.Value);

                case ConditionOperator.NotEquals:
                    return !value.Value.Contains(target.Value);

                default:
                    Debug.LogAssertionFormat("Condition Operator {0} is not supported in InVector3IntArray Condition", condition);
                    return false;
            }
        }

        #endregion

        #region ICopyable

        public override void CopyFrom(Condition original)
        {
            InVector3IntArrayCondition valueCondition = original as InVector3IntArrayCondition;
            useArgument = valueCondition.useArgument;
            value = valueCondition.value;
            condition = valueCondition.condition;
            target = valueCondition.target;
        }

        #endregion
    }
}
