using Celeste.DeckBuilding.Extensions;
using UnityEngine;

namespace Celeste.DeckBuilding.Logic
{
    [CreateAssetMenu(fileName = "SupportsHealth", menuName = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM + "Requirements/Supports Health", order = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM_PRIORITY)]
    public class SupportsHealthEffectRequirement : EffectRequirement
    {
        public override bool Check(EffectRequirementArgs args)
        {
            return args.target.SupportsHealth();
        }
    }
}