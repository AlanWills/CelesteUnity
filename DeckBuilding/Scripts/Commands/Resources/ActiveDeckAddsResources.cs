using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct ActiveDeckAddsResources : IDeckMatchCommand
    {
        private int quantity;

        public ActiveDeckAddsResources(int quantity)
        {
            this.quantity = quantity;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            deckMatchCommandArgs.ActiveDeckRuntime.AddResources(quantity);
        }
    }
}