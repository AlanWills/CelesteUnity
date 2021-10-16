using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct DecreaseMaxArmour : IDeckMatchCommand
    {
        private CardRuntime actorToApplyTo;
        private int maxArmourDecrease;

        public DecreaseMaxArmour(CardRuntime actorToApplyTo, int maxArmourDecrease)
        {
            this.actorToApplyTo = actorToApplyTo;
            this.maxArmourDecrease = maxArmourDecrease;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            actorToApplyTo.DecreaseMaxArmour(maxArmourDecrease);
        }
    }
}