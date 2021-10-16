using Celeste.DeckBuilding.Commands;
using Celeste.DeckBuilding.Events;
using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.DeckBuilding.Cards
{
    [DisplayName("Modify Max Armour Effect")]
    public class ModifyMaxArmourEffectComponent : EffectComponent
    {
        #region Save Data

        [Serializable]
        public class ModifyMaxArmourEffectComponentData : ComponentData
        {
            public int MaxArmourModifier;
        }

        #endregion

        #region Properties and Fields

        [SerializeField] private int initialMaxArmourModifier = 1;

        #endregion

        public override ComponentData CreateData()
        {
            ModifyMaxArmourEffectComponentData modifyMaxArmourData = new ModifyMaxArmourEffectComponentData();
            modifyMaxArmourData.MaxArmourModifier = initialMaxArmourModifier;

            return modifyMaxArmourData;
        }

        public override IDeckMatchCommand UseOn(Instance instance, CardRuntime target)
        {
            int maxArmourModifier = GetMaxArmourModifier(instance);
            if (maxArmourModifier >= 0)
            {
                return new IncreaseMaxArmour(target, maxArmourModifier);
            }
            else
            {
                return new DecreaseMaxArmour(target, Mathf.Abs(maxArmourModifier));
            }
        }

        public int GetMaxArmourModifier(Instance instance)
        {
            ModifyMaxArmourEffectComponentData modifyMaxArmourData = instance.data as ModifyMaxArmourEffectComponentData;
            return modifyMaxArmourData.MaxArmourModifier;
        }
    }
}