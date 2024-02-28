using Celeste.DeckBuilding.Extensions;
using Celeste.Logic;
using Celeste.Tools.Attributes.GUI;
using UnityEngine;

namespace Celeste.DeckBuilding.Logic
{
    [CreateAssetMenu(
        fileName = "CurrentHealth", 
        menuName = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM + "Requirements/Current Health",
        order = CelesteMenuItemConstants.DECKBUILDING_MENU_ITEM_PRIORITY)]
    public class CurrentHealthEffectRequirement : EffectRequirement
    {
        [SerializeField] private ConditionOperator conditionOperator;
        [SerializeField] private bool useMaxHealth;
        [SerializeField, HideIf(nameof(useMaxHealth))] private int targetHealth;

        public override bool Check(EffectRequirementArgs args)
        {
            int current = args.target.GetHealth();
            int target = useMaxHealth ? args.target.GetMaxHealth() : targetHealth;
            return current.SatisfiesComparison(conditionOperator, target);
        }
    }
}