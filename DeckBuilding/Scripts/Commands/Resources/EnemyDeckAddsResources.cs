using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct EnemyDeckAddsResources : IDeckMatchCommand
    {
        private int quantity;

        public EnemyDeckAddsResources(int quantity)
        {
            this.quantity = quantity;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            deckMatchCommandArgs.EnemyDeckRuntime.AddResources(quantity);
        }
    }
}