using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct PlayerDeckDrawsCard : IDeckMatchCommand
    {
        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            deckMatchCommandArgs.PlayerDeckRuntime.DrawCards(1);
        }
    }
}