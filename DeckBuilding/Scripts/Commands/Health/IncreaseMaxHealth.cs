using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct IncreaseMaxHealth : IDeckMatchCommand
    {
        private CardRuntime actorToApplyTo;
        private int maxHealthIncrease;

        public IncreaseMaxHealth(CardRuntime actorToApplyTo, int maxHealthIncrease)
        {
            this.actorToApplyTo = actorToApplyTo;
            this.maxHealthIncrease = maxHealthIncrease;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            actorToApplyTo.IncreaseMaxHealth(maxHealthIncrease);
            actorToApplyTo.AddHealth(maxHealthIncrease);
        }
    }
}