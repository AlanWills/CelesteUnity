using Celeste.DeckBuilding.Cards;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Extensions
{
    public static class CostComponentExtensions
    {
        public static bool SupportsCost(this CardRuntime card)
        {
            return card.HasComponent<CostComponent>();
        }

        public static int GetCost(this CardRuntime card)
        {
            var costComponent = card.FindComponent<CostComponent>();
#if COMPONENT_CHECKS
            if (!costComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Could not find {nameof(CostComponent)} on card {card.CardName}.");
                return 0;
            }
#endif
            return costComponent.component.GetCost(costComponent.instance);
        }

        public static void SetCost(this CardRuntime card, int cost)
        {
            var costComponent = card.FindComponent<CostComponent>();
#if COMPONENT_CHECKS
            if (!costComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to set Cost on a Card with no {nameof(CostComponent)}.");
                return;
            }
#endif
            costComponent.component.SetCost(costComponent.instance, card, cost);
        }

        public static void ModifyCost(this CardRuntime card, int costModifier)
        {
            SetCost(card, GetCost(card) + costModifier);
        }
    }
}