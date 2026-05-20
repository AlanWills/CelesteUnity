using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Commands;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Extensions
{
    public static class EffectComponentExtensions
    {
        public static bool SupportsEffect(this CardInstance card)
        {
            return card.HasComponent<EffectComponent>();
        }

        public static bool CanUseEffectOn(this CardInstance card, CardInstance target)
        {
            var effectComponent = card.FindComponent<EffectComponent>();
#if COMPONENT_CHECKS
            if (!effectComponent.IsValid)
            {
                return false;
            }
#endif
            return effectComponent.component.CanUseOn(effectComponent.instance, card, target);
        }

        public static IDeckMatchCommand UseEffectOn(this CardInstance card, CardInstance target)
        {
            var effectComponent = card.FindComponent<EffectComponent>();
#if COMPONENT_CHECKS
            if (!effectComponent.IsValid)
            {
                return NoOpDeckMatchCommand.IMPL;
            }
#endif
            return effectComponent.component.UseOn(effectComponent.instance, target);
        }

        public static bool EffectRequiresTarget(this CardInstance card)
        {
            var effectComponent = card.FindComponent<EffectComponent>();
#if COMPONENT_CHECKS
            if (!effectComponent.IsValid)
            {
                return false;
            }
#endif
            return effectComponent.component.RequiresTarget(effectComponent.instance);
        }
    }
}