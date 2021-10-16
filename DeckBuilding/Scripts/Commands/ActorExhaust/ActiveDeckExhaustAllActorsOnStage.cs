using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct ActiveDeckExhaustAllActorsOnStage : IDeckMatchCommand
    {
        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            var stage = deckMatchCommandArgs.ActiveDeckRuntime.Stage;
            for (int i = 0, n = stage.NumCards; i < n; ++i)
            {
                deckMatchCommandArgs.CommandDispatcher.AddCommand(new ExhaustCard(stage.GetCard(i)));
            }
        }
    }
}