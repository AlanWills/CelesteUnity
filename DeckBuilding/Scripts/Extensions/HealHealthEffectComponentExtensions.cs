using Celeste.DeckBuilding.Cards;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Extensions
{
    public static class HealHealthEffectComponentExtensions
    {
        public static bool SupportsHealHealthEffect(this CardRuntime card)
        {
            return card.HasComponent<HealHealthEffectComponent>();
        }

        public static int GetHealthToHeal(this CardRuntime card)
        {
            var healHealthComponent = card.FindComponent<HealHealthEffectComponent>();
#if COMPONENT_CHECKS
            if (!healHealthComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Could not find {nameof(HealHealthEffectComponent)} on card {card.CardName}.");
                return 0;
            }
#endif
            return healHealthComponent.component.GetHealthToHeal(healHealthComponent.instance);
        }
    }
}