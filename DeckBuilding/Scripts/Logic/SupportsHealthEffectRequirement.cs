using Celeste.Constants;
using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Extensions;
using Celeste.Logic;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Logic
{
    [CreateAssetMenu(fileName = "SupportsHealth", menuName = "Celeste/Deck Building/Requirements/Supports Health")]
    public class SupportsHealthEffectRequirement : EffectRequirement
    {
        public override bool Check(EffectRequirementArgs args)
        {
            return args.target.SupportsHealth();
        }
    }
}