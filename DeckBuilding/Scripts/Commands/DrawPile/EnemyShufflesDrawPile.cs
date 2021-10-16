using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct EnemyShufflesDrawPile : IDeckMatchCommand
    {
        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            deckMatchCommandArgs.EnemyDeckRuntime.ShuffleDrawPile();
        }
    }
}