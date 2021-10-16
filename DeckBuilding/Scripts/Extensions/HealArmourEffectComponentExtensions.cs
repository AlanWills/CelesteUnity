using Celeste.DeckBuilding.Cards;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Extensions
{
    public static class HealArmourEffectComponentExtensions
    {
        public static bool SupportsHealArmourEffect(this CardRuntime card)
        {
            return card.HasComponent<HealArmourEffectComponent>();
        }

        public static int GetArmourToHeal(this CardRuntime card)
        {
            var healArmourComponent = card.FindComponent<HealArmourEffectComponent>();
#if COMPONENT_CHECKS
            if (!healArmourComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Could not find {nameof(HealArmourEffectComponent)} on card {card.CardName}.");
                return 0;
            }
#endif
            return healArmourComponent.component.GetArmourToHeal(healArmourComponent.instance);
        }
    }
}