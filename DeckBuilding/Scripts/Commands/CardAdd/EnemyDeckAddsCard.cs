using System.Collections;
using UnityEngine;

namespace Celeste.DeckBuilding.Commands
{
    public struct EnemyDeckAddsCard : IDeckMatchCommand
    {
        private CardRuntime card;

        public EnemyDeckAddsCard(CardRuntime card)
        {
            this.card = card;
        }

        public void Execute(DeckMatchCommandArgs deckMatchCommandArgs)
        {
            deckMatchCommandArgs.EnemyDeckRuntime.AddCardToHand(card);
        }
    }
}