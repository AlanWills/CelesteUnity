using Celeste.DeckBuilding.Extensions;
using UnityEngine;

namespace Celeste.DeckBuilding.Logic
{
    [CreateAssetMenu(fileName = "SupportsArmour", menuName = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM + "Requirements/Supports Armour", order = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM_PRIORITY)]
    public class SupportsArmourEffectRequirement : EffectRequirement
    {
        public override bool Check(EffectRequirementArgs args)
        {
            return args.target.SupportsArmour();
        }
    }
}