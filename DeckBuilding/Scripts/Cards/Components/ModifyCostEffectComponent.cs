using Celeste.DeckBuilding.Commands;
using Celeste.DeckBuilding.Events;
using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Cards
{
    [DisplayName("Modify Cost Effect")]
    public class ModifyCostEffectComponent : EffectComponent
    {
        #region Save Data

        [Serializable]
        public class ModifyCostEffectComponentData : ComponentData
        {
            public int CostModifier;
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private int initialCostModifier = 1;

        #endregion

        public override ComponentData CreateData()
        {
            ModifyCostEffectComponentData modifyCostData = new ModifyCostEffectComponentData();
            modifyCostData.CostModifier = initialCostModifier;

            return modifyCostData;
        }

        public override IDeckMatchCommand UseOn(Instance instance, CardRuntime target)
        {
            return new ModifyCost(target, GetCostModifier(instance));
        }

        public int GetCostModifier(Instance instance)
        {
            ModifyCostEffectComponentData healHealthData = instance.data as ModifyCostEffectComponentData;
            return healHealthData.CostModifier;
        }
    }
}