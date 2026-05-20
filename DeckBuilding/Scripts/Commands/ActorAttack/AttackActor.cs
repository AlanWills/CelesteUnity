using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct AttackActor : IDeckMatchCommand
    {
        private CardInstance attacker;
        private CardInstance target;

        public AttackActor(CardInstance attacker, CardInstance target)
        {
            this.attacker = attacker;
            this.target = target;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            deckMatchCommandArgs.CommandDispatcher.AddCommand(new ApplyDamage(target, attacker.GetStrength()));
        }
    }
}