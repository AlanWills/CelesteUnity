using Celeste.Constants;
using Celeste.Logic;
using Celeste.Tools.Attributes.GUI;
using UnityEngine;

namespace Celeste.DeckBuilding.Logic
{
    [CreateAssetMenu(fileName = "CardOwner", menuName = "Celeste/Deck Building/Requirements/Card Owner")]
    public class CardOwnerEffectRequirement : EffectRequirement
    {
        [SerializeField] private ConditionOperator conditionOperator;
        [SerializeField] private bool useEffectOwner;
        [SerializeField, HideIf(nameof(useEffectOwner))] private ID owner;

        public override bool Check(EffectRequirementArgs args)
        {
            ID current = useEffectOwner ? args.effect.Owner : owner;
            return args.target.Owner.SatisfiesEquality(conditionOperator, current);
        }
    }
}