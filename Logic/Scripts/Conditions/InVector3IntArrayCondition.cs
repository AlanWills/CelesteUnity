using Celeste.Events;
using Celeste.Parameters;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Logic
{
    [CreateAssetMenu(fileName = "InVector3IntArrayCondition", menuName = "Celeste/Logic/In Vector3 Int Array Condition")]
    [DisplayName("In Vector3 Int Array")]
    public class InVector3IntArrayCondition : Condition
    {
        #region Properties and Fields

        public Vector3IntArrayValue value;
        public ConditionOperator condition;
        public Vector3IntReference target;

        #endregion

        #region Init Methods

        protected override void DoInitialize()
        {
            if (target == null)
            {
                target = CreateDependentAsset<Vector3IntReference>($"{name}_target");
            }

            if (target.IsConstant)
            {
                target.ReferenceValue.AddValueChangedCallback(OnTargetChangedCallback);
            }
        }

        #endregion

        #region Check Methods

        public override void SetVariable(object arg)
        {
            target.IsConstant = true;
            target.Value = arg != null ? (Vector3Int)arg : default;
        }

        protected override bool DoCheck()
        {
            switch (condition)
            {
                case ConditionOperator.Equals:
                    return value.Value.Contains(target.Value);

                case ConditionOperator.NotEquals:
                    return !value.Value.Contains(target.Value);

                default:
                    UnityEngine.Debug.LogAssertionFormat("Condition Operator {0} is not supported in InVector3IntArray Condition", condition);
                    return false;
            }
        }

        #endregion

        #region ICopyable

        public override void CopyFrom(Condition original)
        {
            InVector3IntArrayCondition valueCondition = original as InVector3IntArrayCondition;
            value = valueCondition.value;
            condition = valueCondition.condition;
            target = valueCondition.target;
        }

        #endregion

        #region Callbacks

        private void OnTargetChangedCallback(ValueChangedArgs<Vector3Int> args)
        {
            Check();
        }

        #endregion
    }
}
