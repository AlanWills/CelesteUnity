using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct RefreshCard : IDeckMatchCommand
    {
        private CardRuntime card;

        public RefreshCard(CardRuntime card)
        {
            this.card = card;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            card.Refresh();
        }
    }
}