using Celeste.DeckBuilding.Cards;
using Celeste.Logic;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Logic
{
    public struct EffectRequirementArgs
    {
        public CardInstance effect;
        public CardInstance target;
    }

    public abstract class EffectRequirement : ScriptableObject, IRequirement<EffectRequirementArgs>
    {
        public abstract bool Check(EffectRequirementArgs args);
    }
}