using Celeste.DeckBuilding.Commands;
using Celeste.DeckBuilding.Events;
using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Cards
{
    [DisplayName("Modify Max Health Effect")]
    public class ModifyMaxHealthEffectComponent : EffectComponent
    {
        #region Save Data

        [Serializable]
        public class ModifyMaxHealthEffectComponentData : ComponentData
        {
            public int MaxHealthModifier;
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private int initialMaxHealthModifier = 1;

        #endregion

        public override ComponentData CreateData()
        {
            ModifyMaxHealthEffectComponentData modifyMaxHealthData = new ModifyMaxHealthEffectComponentData();
            modifyMaxHealthData.MaxHealthModifier = initialMaxHealthModifier;

            return modifyMaxHealthData;
        }

        public override IDeckMatchCommand UseOn(Instance instance, CardRuntime target)
        {
            int maxHealthModifier = GetMaxHealthModifier(instance);
            if (maxHealthModifier >= 0)
            {
                return new IncreaseMaxHealth(target, maxHealthModifier);
            }
            else
            {
                return new DecreaseMaxHealth(target, Mathf.Abs(maxHealthModifier));
            }
        }

        public int GetMaxHealthModifier(Instance instance)
        {
            ModifyMaxHealthEffectComponentData modifyMaxHealthData = instance.data as ModifyMaxHealthEffectComponentData;
            return modifyMaxHealthData.MaxHealthModifier;
        }
    }
}