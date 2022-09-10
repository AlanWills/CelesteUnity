using Celeste.Events;
using Celeste.Logic;
using Celeste.Parameters;
using System.Collections;
using UnityEngine;

namespace Celeste.Inventory.Logic
{
    [CreateAssetMenu(fileName = nameof(InventoryItemCountCondition), menuName = "Celeste/Inventory/Logic/Inventory Item Count Condition")]
    public class InventoryItemCountCondition : Condition
    {
        #region Properties and Fields

        [SerializeField] private InventoryRecord inventoryRecord;
        [SerializeField] private InventoryItem itemToFind;
        [SerializeField] private ConditionOperator condition;
        [SerializeField] private IntReference target;

        #endregion

        protected override void DoInit()
        {
            if (target == null)
            {
                target = CreateDependentAsset<IntReference>($"{name}_target");
            }

            if (target.IsConstant)
            {
                target.ReferenceValue.AddValueChangedCallback(OnValueChangedCallback);
            }
        }

        protected override void DoShutdown()
        {
            if (target.IsConstant)
            {
                target.ReferenceValue.RemoveValueChangedCallback(OnValueChangedCallback);
            }
        }

        public override void CopyFrom(Condition original)
        {
            InventoryItemCountCondition itemCondition = original as InventoryItemCountCondition;
            inventoryRecord = itemCondition.inventoryRecord;
            itemToFind = itemCondition.itemToFind;
            condition = itemCondition.condition;
            target.CopyFrom(itemCondition.target);
        }

        protected override bool DoCheck()
        {
            int count = 0;

            for (int i = 0, n = inventoryRecord.NumItems; i < n; ++i)
            {
                if (inventoryRecord.GetItem(i) == itemToFind)
                {
                    ++count;
                }
            }

            return count.SatisfiesComparison(condition, target.Value);
        }

        public override void SetTarget(object arg)
        {
            target.IsConstant = true;
            target.Value = arg != null ? (int)arg : default;
        }

        #region Callbacks

        private void OnValueChangedCallback(ValueChangedArgs<int> args)
        {
            Check();
        }

        #endregion
    }
}