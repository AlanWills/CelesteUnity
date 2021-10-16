using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct PlayerDeckAddsResources : IDeckMatchCommand
    {
        private int quantity;

        public PlayerDeckAddsResources(int quantity)
        {
            this.quantity = quantity;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            deckMatchCommandArgs.PlayerDeckRuntime.AddResources(quantity);
        }
    }
}