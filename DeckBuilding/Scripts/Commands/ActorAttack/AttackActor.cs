using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct AttackActor : IDeckMatchCommand
    {
        private CardRuntime attacker;
        private CardRuntime target;

        public AttackActor(CardRuntime attacker, CardRuntime target)
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