﻿using Celeste.DeckBuilding.Extensions;
using Celeste.Logic;
using Celeste.Tools.Attributes.GUI;
using UnityEngine;

namespace Celeste.DeckBuilding.Logic
{
    [CreateAssetMenu(
        fileName = "CurrentArmour", 
        menuName = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM + "Requirements/Current Armour",
        order = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM_PRIORITY)]
    public class CurrentArmourEffectRequirement : EffectRequirement
    {
        [SerializeField] private ConditionOperator conditionOperator;
        [SerializeField] private bool useMaxArmour;
        [SerializeField, HideIf(nameof(useMaxArmour))] private int targetArmour;

        public override bool Check(EffectRequirementArgs args)
        {
            int current = args.target.GetArmour();
            int target = useMaxArmour ? args.target.GetMaxArmour() : targetArmour;
            return current.SatisfiesComparison(conditionOperator, target);
        }
    }
}