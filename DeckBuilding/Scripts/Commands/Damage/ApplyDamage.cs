using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct ApplyDamage : IDeckMatchCommand
    {
        private CardRuntime actorToApplyTo;
        private int damage;

        public ApplyDamage(CardRuntime actorToApplyTo, int damage)
        {
            this.actorToApplyTo = actorToApplyTo;
            this.damage = damage;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            actorToApplyTo.ApplyDamage(damage);
        }
    }
}