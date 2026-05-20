using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct IncreaseMaxArmour : IDeckMatchCommand
    {
        private CardInstance actorToApplyTo;
        private int maxArmourIncrease;

        public IncreaseMaxArmour(CardInstance actorToApplyTo, int maxArmourIncrease)
        {
            this.actorToApplyTo = actorToApplyTo;
            this.maxArmourIncrease = maxArmourIncrease;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            actorToApplyTo.IncreaseMaxArmour(maxArmourIncrease);
            actorToApplyTo.AddArmour(maxArmourIncrease);
        }
    }
}