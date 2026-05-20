using Celeste.DeckBuilding.Extensions;
using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct RefreshCard : IDeckMatchCommand
    {
        private CardInstance card;

        public RefreshCard(CardInstance card)
        {
            this.card = card;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            card.Refresh();
        }
    }
}