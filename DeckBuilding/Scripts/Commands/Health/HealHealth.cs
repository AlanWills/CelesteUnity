using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct HealHealth : IDeckMatchCommand
    {
        private CardRuntime actorToApplyTo;
        private int healthToHeal;

        public HealHealth(CardRuntime actorToApplyTo, int healthToHeal)
        {
            this.actorToApplyTo = actorToApplyTo;
            this.healthToHeal = healthToHeal;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            actorToApplyTo.AddHealth(healthToHeal);
        }
    }
}