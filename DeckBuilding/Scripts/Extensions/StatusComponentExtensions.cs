using Celeste.DeckBuilding.Cards;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Extensions
{
    public static class StatusComponentExtensions
    {
        public static bool SupportsStatus(this CardRuntime card)
        {
            return card.HasComponent<StatusComponent>();
        }

        public static bool HasCardStatus(this CardRuntime card, CardStatus cardStatus)
        {
            var statusComponent = card.FindComponent<StatusComponent>();
            return statusComponent.IsValid && statusComponent.component.HasCardStatus(statusComponent.instance, cardStatus);
        }

        public static bool IsRemovedFromDeckWhenPlayed(this CardRuntime card)
        {
            return card.HasCardStatus(CardStatus.RemovedFromDeckWhenPlayed);
        }
    }
}