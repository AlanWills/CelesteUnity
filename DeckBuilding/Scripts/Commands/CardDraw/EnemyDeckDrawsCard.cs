using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct EnemyDeckDrawsCard : IDeckMatchCommand
    {
        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            deckMatchCommandArgs.EnemyDeckRuntime.DrawCards(1);
        }
    }
}