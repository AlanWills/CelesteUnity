using Celeste.DeckBuilding.Cards;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Extensions
{
    public static class ModifyCostEffectComponentExtensions
    {
        public static bool SupportsModifyCostEffect(this CardRuntime card)
        {
            return card.HasComponent<ModifyCostEffectComponent>();
        }
    }
}