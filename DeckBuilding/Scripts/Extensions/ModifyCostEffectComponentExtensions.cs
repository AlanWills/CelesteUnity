using Celeste.DeckBuilding.Cards;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Extensions
{
    public static class ModifyCostEffectComponentExtensions
    {
        public static bool SupportsModifyCostEffect(this CardInstance card)
        {
            return card.HasComponent<ModifyCostEffectComponent>();
        }
    }
}