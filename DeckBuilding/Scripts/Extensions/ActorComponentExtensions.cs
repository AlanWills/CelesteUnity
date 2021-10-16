using Celeste.DeckBuilding.Cards;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Extensions
{
    public static class ActorComponentExtensions
    {
        public static bool SupportsActor(this CardRuntime card)
        {
            return card.HasComponent<ActorComponent>();
        }

        public static bool IsOnStage(this CardRuntime card)
        {
            var actorComponent = card.FindComponent<ActorComponent>();
#if COMPONENT_CHECKS
            if (!actorComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Could not find {nameof(ActorComponent)} on card {card.CardName}.");
                return false;
            }
#endif
            return actorComponent.component.IsOnStage(actorComponent.instance);
        }

        public static void SetOnStage(this CardRuntime card, bool isOnStage)
        {
            var actorComponent = card.FindComponent<ActorComponent>();
#if COMPONENT_CHECKS
            if (!actorComponent.IsValid)
            {
                UnityEngine.Debug.LogAssertion($"Attempting to set OnStage on a Card with no {nameof(ActorComponent)}.");
                return;
            }
#endif
            actorComponent.component.SetOnStage(actorComponent.instance, isOnStage);
        }
    }
}