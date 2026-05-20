using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct ModifyCost : IDeckMatchCommand
    {
        private CardInstance actorToApplyTo;
        private int costModifier;

        public ModifyCost(CardInstance actorToApplyTo, int costModifier)
        {
            this.actorToApplyTo = actorToApplyTo;
            this.costModifier = costModifier;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            actorToApplyTo.AddHealth(costModifier);
        }
    }
}