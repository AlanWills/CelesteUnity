using Celeste.DeckBuilding.Commands;
using Celeste.DeckBuilding.Events;
using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Cards
{
    [DisplayName("Heal Armour Effect")]
    public class HealArmourEffectComponent : EffectComponent
    {
        #region Save Data

        [Serializable]
        public class HealArmourEffectComponentData : ComponentData
        {
            public int ArmourToHeal;
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private int initialArmourToHeal = 1;

        #endregion

        public override ComponentData CreateData()
        {
            HealArmourEffectComponentData healArmourData = new HealArmourEffectComponentData();
            healArmourData.ArmourToHeal = initialArmourToHeal;

            return healArmourData;
        }

        public override IDeckMatchCommand UseOn(Instance instance, CardRuntime target)
        {
            return new HealArmour(target, GetArmourToHeal(instance));
        }

        public int GetArmourToHeal(Instance instance)
        {
            HealArmourEffectComponentData healArmourData = instance.data as HealArmourEffectComponentData;
            return healArmourData.ArmourToHeal;
        }
    }
}