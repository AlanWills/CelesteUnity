using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct DecreaseMaxArmour : IDeckMatchCommand
    {
        private CardInstance actorToApplyTo;
        private int maxArmourDecrease;

        public DecreaseMaxArmour(CardInstance actorToApplyTo, int maxArmourDecrease)
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