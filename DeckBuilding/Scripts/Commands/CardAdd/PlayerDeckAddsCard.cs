using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct PlayerDeckAddsCard : IDeckMatchCommand
    {
        private CardInstance card;

        public PlayerDeckAddsCard(CardInstance card)
        {
            this.card = card;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            deckMatchCommandArgs.PlayerDeckRuntime.AddCardToHand(card);
        }
    }
}