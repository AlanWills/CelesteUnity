using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct DecreaseMaxHealth : IDeckMatchCommand
    {
        private CardRuntime actorToApplyTo;
        private int maxHealthDecrease;

        public DecreaseMaxHealth(CardRuntime actorToApplyTo, int maxHealthDecrease)
        {
            this.actorToApplyTo = actorToApplyTo;
            this.maxHealthDecrease = maxHealthDecrease;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            actorToApplyTo.DecreaseMaxHealth(maxHealthDecrease);
        }
    }
}