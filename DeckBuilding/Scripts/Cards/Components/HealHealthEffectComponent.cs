using Celeste.Components;
using Celeste.DeckBuilding.Commands;
using System;
using System.ComponentModel;
using UnityEngine;

namespace Celeste.DeckBuilding.Cards
{
    [DisplayName("Heal Health Effect")]
    public class HealHealthEffectComponent : EffectComponent
    {
        #region Save Data

        [Serializable]
        public class HealHealthEffectComponentData : ComponentData
        {
            public int HealthToHeal;
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private int initialHealthToHeal = 1;

        #endregion

        public override ComponentData CreateData()
        {
            HealHealthEffectComponentData healHealthData = new HealHealthEffectComponentData();
            healHealthData.HealthToHeal = initialHealthToHeal;

            return healHealthData;
        }

        public override IDeckMatchCommand UseOn(Instance instance, CardRuntime target)
        {
            return new HealHealth(target, GetHealthToHeal(instance));
        }

        public int GetHealthToHeal(Instance instance)
        {
            HealHealthEffectComponentData healHealthData = instance.data as HealHealthEffectComponentData;
            return healHealthData.HealthToHeal;
        }
    }
}