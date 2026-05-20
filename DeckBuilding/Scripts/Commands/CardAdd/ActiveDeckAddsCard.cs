using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct ActiveDeckAddsCard : IDeckMatchCommand
    {
        private CardInstance card;

        public ActiveDeckAddsCard(CardInstance card)
        {
            this.card = card;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            deckMatchCommandArgs.ActiveDeckRuntime.AddCardToHand(card);
        }
    }
}