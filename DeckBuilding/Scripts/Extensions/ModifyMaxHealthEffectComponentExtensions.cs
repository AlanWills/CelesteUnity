using Celeste.DeckBuilding.Cards;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Extensions
{
    public static class ModifyMaxHealthEffectComponentExtensions
    {
        public static bool SupportsModifyMaxHealthEffect(this CardRuntime card)
        {
            return card.HasComponent<ModifyMaxHealthEffectComponent>();
        }

        public static int GetMaxArmourModifier(this CardRuntime card)
        {
            var modifyMaxHealthComponent = card.FindComponent<ModifyMaxHealthEffectComponent>();
#if COMPONENT_CHECKS
            if (!modifyMaxHealthComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Could not find {nameof(ModifyMaxHealthEffectComponent)} on card {card.CardName}.");
                return 0;
            }
#endif
            return modifyMaxHealthComponent.component.GetMaxHealthModifier(modifyMaxHealthComponent.instance);
        }
    }
}