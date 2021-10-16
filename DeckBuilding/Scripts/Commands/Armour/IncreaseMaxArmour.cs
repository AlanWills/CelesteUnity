using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct IncreaseMaxArmour : IDeckMatchCommand
    {
        private CardRuntime actorToApplyTo;
        private int maxArmourIncrease;

        public IncreaseMaxArmour(CardRuntime actorToApplyTo, int maxArmourIncrease)
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