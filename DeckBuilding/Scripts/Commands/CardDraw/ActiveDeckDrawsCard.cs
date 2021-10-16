using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct ActiveDeckDrawsCard : IDeckMatchCommand
    {
        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            deckMatchCommandArgs.ActiveDeckRuntime.DrawCards(1);
        }
    }
}