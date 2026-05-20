using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct IncreaseMaxHealth : IDeckMatchCommand
    {
        private CardInstance actorToApplyTo;
        private int maxHealthIncrease;

        public IncreaseMaxHealth(CardInstance actorToApplyTo, int maxHealthIncrease)
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