using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct PlayerDeckAddsCard : IDeckMatchCommand
    {
        private CardRuntime card;

        public PlayerDeckAddsCard(CardRuntime card)
        {
            this.card = card;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            deckMatchCommandArgs.PlayerDeckRuntime.AddCardToHand(card);
        }
    }
}