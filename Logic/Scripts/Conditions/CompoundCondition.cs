using Celeste.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.Logic
{
    [CreateAssetMenu(fileName = nameof(CompoundCondition), menuName = CelesteMenuItemConstants.LOGIC_MENU_ITEM + "Compound Condition", order = CelesteMenuItemConstants.LOGIC_MENU_ITEM_PRIORITY)]
    [DisplayName("Compound")]
    public class CompoundCondition : Condition
    {
        #region Enums

        [Serializable]
        public enum LogicalOperator
        {
            And,
            Or
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private LogicalOperator logicalOperator;
        [SerializeField] private List<Condition> conditions = new();

        #endregion

        public override void CopyFrom(Condition original)
        {
            CompoundCondition compoundCondition = original as CompoundCondition;
            conditions.AddRange(compoundCondition.conditions);
        }

        protected override void DoSetVariable(object arg)
        {
            // No-op
        }

        protected override void DoInitialize()
        {
            if (Application.isPlaying)
            {
                for (int i = 0, n = conditions.Count; i < n; i++)
                {
                    conditions[i].AddOnIsMetConditionChanged(OnConditionValueChanged);
                }
            }
        }

        protected override void DoShutdown()
        {
            if (Application.isPlaying)
            {
                for (int i = 0, n = conditions.Count; i < n; i++)
                {
                    conditions[i].RemoveOnIsMetConditionChanged(OnConditionValueChanged);
                }
            }
        }

        protected override bool DoCheck()
        {
            // Handle the trivial case of no conditions here, by returning true
            // Otherwise, this starts out as false and must be made true by a condition
            bool isTrue = conditions.Count == 0;

            for (int i = 0, n = conditions.Count; i < n; ++i)
            {
                Condition condition = conditions[i];
                bool conditionTrue = condition.IsMet;

                if (!conditionTrue && logicalOperator == LogicalOperator.And)
                {
                    // At least one of the conditions was false and we require all must be true
                    return false;
                }

                isTrue |= condition.IsMet;
            }

            return isTrue;
        }

        #region Callbacks

        private void OnConditionValueChanged(ValueChangedArgs<bool> args)
        {
            Check();
        }

        #endregion
    }
}
