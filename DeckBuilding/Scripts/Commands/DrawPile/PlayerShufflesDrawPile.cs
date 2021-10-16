using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct PlayerShufflesDrawPile : IDeckMatchCommand
    {
        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            deckMatchCommandArgs.PlayerDeckRuntime.ShuffleDrawPile();
        }
    }
}