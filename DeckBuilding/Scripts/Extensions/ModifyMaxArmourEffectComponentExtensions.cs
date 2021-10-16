using Celeste.DeckBuilding.Cards;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Extensions
{
    public static class ModifyMaxArmourEffectComponentExtensions
    {
        public static bool SupportsModifyMaxArmourEffect(this CardRuntime card)
        {
            return card.HasComponent<ModifyMaxArmourEffectComponent>();
        }

        public static int GetMaxArmourModifier(this CardRuntime card)
        {
            var modifyMaxArmourComponent = card.FindComponent<ModifyMaxArmourEffectComponent>();
#if COMPONENT_CHECKS
            if (!modifyMaxArmourComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Could not find {nameof(ModifyMaxArmourEffectComponent)} on card {card.CardName}.");
                return 0;
            }
#endif
            return modifyMaxArmourComponent.component.GetMaxArmourModifier(modifyMaxArmourComponent.instance);
        }
    }
}