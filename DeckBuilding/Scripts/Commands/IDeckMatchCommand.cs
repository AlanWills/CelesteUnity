using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct DeckMatchCommandArgs
    {
        public DeckMatchCommandDispatcher CommandDispatcher
        {
            get { return commandDispatcher; }
        }

        public DeckMatchRuntime DeckMatchRuntime
        {
            get { return deckMatchRuntime; }
        }

        public DeckRuntime ActiveDeckRuntime
        {
            get { return deckMatchRuntime.ActiveDeckRuntime; }
        }

        public DeckRuntime InactiveDeckRuntime
        {
            get { return deckMatchRuntime.InactiveDeckRuntime; }
        }

        public DeckRuntime PlayerDeckRuntime
        {
            get { return deckMatchRuntime.PlayerDeckRuntime; }
        }

        public DeckRuntime EnemyDeckRuntime
        {
            get { return deckMatchRuntime.InactiveDeckRuntime; }
        }

        private DeckMatchCommandDispatcher commandDispatcher;
        private DeckMatchRuntime deckMatchRuntime;

        public DeckMatchCommandArgs(DeckMatchCommandDispatcher commandDispatcher, DeckMatchRuntime deckMatchRuntime)
        {
            this.commandDispatcher = commandDispatcher;
            this.deckMatchRuntime = deckMatchRuntime;
        }
    }

    public struct NoOpDeckMatchCommand : IDeckMatchCommand
    {
        public static readonly IDeckMatchCommand IMPL = new NoOpDeckMatchCommand();

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs) { }
    }

    public interface IDeckMatchCommand
    {
        void Execute(DeckMatchCommandArgs deckMatchCommandArgs);
    }
}